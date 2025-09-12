using Application.Common.Pagination;
using Domain.Entities.Enums;
using System.Text.Json.Serialization;

namespace Application.Dtos.Response.ProfessorDto;

public class GetProfessorDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = String.Empty;
    public string LastName { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;
    public int StudentsCount { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ProfessorRank Status { get; set; }
    public DateTime HireDate { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ProfessorsPageResponseDto
{
    public PaginationInfo Meta { get; set; } = PaginationInfo.Empty;
    public List<GetProfessorDto> Items { get; set; } = new();
}
