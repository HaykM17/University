using Application.Common.Pagination;
using Domain.Entities.Enums;
using System.Text.Json.Serialization;

namespace Application.Dtos.Response.StudentDto;

public class GetStudentsResponseDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = String.Empty;
    public string LastName { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;
    public int ProfessorsCount { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EnrollmentStatus Status { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class StudentsPageResponseDto
{
    public PaginationInfo Meta { get; set; } = PaginationInfo.Empty;
    public List<GetStudentsResponseDto> Items { get; set; } = new();
}