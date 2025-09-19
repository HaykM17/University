using Application.Common.Pagination;
using Domain.Entities;

namespace Application.Common.Extensions;

public static class StudentQueryableExtensions
{
    public static IQueryable<Student> Paginate(this IQueryable<Student> query, FilterOptions? filter)
    {
        if (filter is null) return query;

        var sortBy = filter.SortBy ?? "";
        var order = filter.SortOrder ?? SortOrders.Descending;

        if (string.Equals(sortBy, "name", StringComparison.OrdinalIgnoreCase))
        {
            return QueryableExtensionsPagination.Paginate(
                query, filter,
                q => string.Equals(order, SortOrders.Ascending, StringComparison.OrdinalIgnoreCase)
                    ? q.OrderBy(s => s.LastName).ThenBy(s => s.FirstName)
                    : q.OrderByDescending(s => s.LastName).ThenByDescending(s => s.FirstName)
            );
        }

        if (string.Equals(sortBy, "email", StringComparison.OrdinalIgnoreCase))
        {
            return QueryableExtensionsPagination.Paginate(
                query, filter,
                q => q.OrderBy(order, s => s.Email)
            );
        }

        if (string.Equals(sortBy, "status", StringComparison.OrdinalIgnoreCase))
        {
            return QueryableExtensionsPagination.Paginate(
                query, filter,
                q => q.OrderBy(order, s => s.Status)
            );
        }

        if (string.Equals(sortBy, "professors", StringComparison.OrdinalIgnoreCase))
        {
            return QueryableExtensionsPagination.Paginate(
                query, filter,
                q => q.OrderBy(order, s => s.ProfessorStudents.Count)
            );
        }

        return QueryableExtensionsPagination.Paginate(query, filter);
    }
}