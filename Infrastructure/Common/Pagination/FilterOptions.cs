namespace Vista.Los.Platform.EntityFrameworkCore.Pagination;

public record FilterOptions(
    int Page = 1,
    int PerPage = 10,
    string SortBy = "CreatedAt",
    string SortOrder = SortOrders.Descending
)
{
    public static FilterOptions Default => new();
}

public record FilterOptions<TFilterCriteria>(
    int Page = 1,
    int PerPage = 10,
    string SortBy = "CreatedAt",
    string SortOrder = SortOrders.Descending,
    TFilterCriteria? Filters = null
) : FilterOptions(Page, PerPage, SortBy, SortOrder) where TFilterCriteria : class;

public static class SortOrders
{
    public const string Ascending = "asc";
    public const string Descending = "desc";
}
