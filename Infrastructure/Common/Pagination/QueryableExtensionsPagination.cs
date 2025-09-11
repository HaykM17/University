using System.Linq.Expressions;

namespace Vista.Los.Platform.EntityFrameworkCore.Pagination;

public static partial class QueryableExtensionsPagination
{
    public static IQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> query, string orderBy, Expression<Func<TSource, TKey>> keySelector)
    {
        return orderBy switch
        {
            SortOrders.Descending => query.OrderByDescending(keySelector),
            SortOrders.Ascending => query.OrderBy(keySelector),
            _ => query
        };
    }

    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, FilterOptions? filter)
    {
        if (filter == null)
            return query;

        return Paginate(query, filter, query => ApplySorting(query, filter.SortBy, filter.SortOrder));
    }

    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, FilterOptions? filter, Func<IQueryable<T>, IQueryable<T>> orderBy)
    {
        if (filter == null)
            return query;

        // Apply sorting
        query = orderBy(query);

        // Apply pagination
        int skip = (filter.Page - 1) * filter.PerPage;
        query = query.Skip(skip).Take(filter.PerPage);

        return query;
    }

    private static IQueryable<T> ApplySorting<T>(IQueryable<T> query, string sortBy, string sortOrder)
    {
        if (string.IsNullOrWhiteSpace(sortBy))
        {
            return query; // If no sorting field provided, return unsorted query
        }

        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, sortBy);
        var lambda = Expression.Lambda(property, parameter);

        string methodName = sortOrder?.ToLower() == SortOrders.Descending ?
            nameof(Queryable.OrderByDescending) : nameof(Queryable.OrderBy);
        var resultExpression = Expression.Call(typeof(Queryable), methodName,
            [query.ElementType, property.Type],
            query.Expression, Expression.Quote(lambda));

        return query.Provider.CreateQuery<T>(resultExpression);
    }
}