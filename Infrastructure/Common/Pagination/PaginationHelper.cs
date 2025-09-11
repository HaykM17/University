namespace Vista.Los.Platform.EntityFrameworkCore.Pagination;

public static class PaginationHelper
{
    public static int CalculateTotalPages(int totalCount, int perPage)
    {
        if (perPage <= 0)
            throw new ArgumentOutOfRangeException(nameof(perPage), "Items per page must be greater than zero.");

        return (totalCount + perPage - 1) / perPage;
    }
}