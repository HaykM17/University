using Application.Common.Pagination;
using Domain.Entities.Enums;

public record GetStudentsFilterRequestDto(
    string? Name = null,
    string? Email = null,
    EnrollmentStatus? Status = null,
    int? MinProfessors = null
);

public record GetStudentsRequestDto(
    int Page = 1,
    int PerPage = 10,
    string SortBy = "CreatedAt",
    string SortOrder = SortOrders.Descending,
    GetStudentsFilterRequestDto? Filters = null
) : FilterOptions<GetStudentsFilterRequestDto>(Page, PerPage, SortBy, SortOrder, Filters);