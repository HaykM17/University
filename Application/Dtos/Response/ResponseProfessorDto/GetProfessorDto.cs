using Application.Common.Pagination;
using Domain.Entities.Enums;

namespace Application.Dtos.Response.ResponseProfessorDto;

public class GetProfessorDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public int StudentsCount { get; set; }   // ProfessorStudents.Count
    public ProfessorRank Status { get; set; }          // enum как int (можно заменить на string)
    public DateTime HireDate { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ProfessorsPageResponseDto
{
    public PaginationInfo Meta { get; set; } = PaginationInfo.Empty;
    public List<GetProfessorDto> Items { get; set; } = new();
}
