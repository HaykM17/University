namespace Application.Common.Pagination;

/// <summary>
/// Metadata about the paginated response
/// </summary>
public class PaginationInfo
{
    public static PaginationInfo Empty { get; } = new()
    {
        Total = 0,
        CurrentPage = 0,
        TotalPages = 0
    };

    /// <summary> Total number of entities. </summary>
    public int Total { get; set; }

    /// <summary> Current page number. </summary>
    public int CurrentPage { get; set; }

    /// <summary> Total number of pages (calculated based on total and perPage). </summary>
    public int TotalPages { get; set; }

    public static PaginationInfo Create(int total, int page, int perPage) => new()
    {
        Total = total,
        CurrentPage = page,
        TotalPages = (int)Math.Ceiling((double)total / perPage)
    };

    public static PaginationInfo Create(int total, FilterOptions filterOptions)
        => Create(total, filterOptions.Page, filterOptions.PerPage);
}
