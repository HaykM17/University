using Application.Common.Pagination;
using Domain.Entities.Enums;

public record GetProfessorsFilterRequestDto(
    string? Name = null,
    string? Email = null,
    ProfessorRank? Status = null,
    int? MinStudents = null
);

public record GetProfessorsRequestDto(
    int Page = 1,
    int PerPage = 10,
    string SortBy = "CreatedAt",
    string SortOrder = SortOrders.Descending,
    GetProfessorsFilterRequestDto? Filters = null
) : FilterOptions<GetProfessorsFilterRequestDto>(Page, PerPage, SortBy, SortOrder, Filters);