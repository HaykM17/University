using Application.Common.Pagination;
using Domain.Entities.Enums;

public record GetProfessorsFilterRequestDto(
    string? Name = null,            // точное имя/фамилия (FirstName/LastName)
    string? Email = null,           // точный email
    ProfessorRank? Status = null,   // звание/статус
    int? MinStudents = null         // минимальное количество студентов у профессора
);

// ЕДИНЫЙ вход для контроллера. Наследуемся от FilterOptions<TFilters>.
public record GetProfessorsRequestDto(
    int Page = 1,
    int PerPage = 10,
    string SortBy = "CreatedAt",    // ВАЖНО: в твоём entity поле называется CreatedAt
    string SortOrder = SortOrders.Descending,
    GetProfessorsFilterRequestDto? Filters = null
) : FilterOptions<GetProfessorsFilterRequestDto>(Page, PerPage, SortBy, SortOrder, Filters);
