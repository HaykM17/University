using Application.Common.Pagination;
using Domain.Entities;

namespace Application.Common.Extensions;

public static class ProfessorQueryableExtensions
{
    // Поддерживает SortBy: "name" (LastName, FirstName), "email", "status", "students"
    // Для остальных полей (Id, CreatedAt, HireDate) — динамика по имени свойства.
    public static IQueryable<Professor> Paginate(this IQueryable<Professor> query, FilterOptions? filter)
    {
        if (filter is null) return query;

        var sortBy = filter.SortBy ?? "";
        var order = filter.SortOrder ?? SortOrders.Descending;

        if (string.Equals(sortBy, "name", StringComparison.OrdinalIgnoreCase))
        {
            return QueryableExtensionsPagination.Paginate(
                query, filter,
                q => string.Equals(order, SortOrders.Ascending, StringComparison.OrdinalIgnoreCase)
                    ? q.OrderBy(p => p.LastName).ThenBy(p => p.FirstName)
                    : q.OrderByDescending(p => p.LastName).ThenByDescending(p => p.FirstName)
            );
        }

        if (string.Equals(sortBy, "email", StringComparison.OrdinalIgnoreCase))
        {
            return QueryableExtensionsPagination.Paginate(
                query, filter,
                q => q.OrderBy(order, p => p.Email)
            );
        }

        if (string.Equals(sortBy, "status", StringComparison.OrdinalIgnoreCase))
        {
            return QueryableExtensionsPagination.Paginate(
                query, filter,
                q => q.OrderBy(order, p => p.Status)
            );
        }

        if (string.Equals(sortBy, "students", StringComparison.OrdinalIgnoreCase))
        {
            return QueryableExtensionsPagination.Paginate(
                query, filter,
                q => q.OrderBy(order, p => p.ProfessorStudents.Count)
            );
        }

        return QueryableExtensionsPagination.Paginate(query, filter);
    }
}