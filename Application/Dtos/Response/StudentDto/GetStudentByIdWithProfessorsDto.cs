using Application.Dtos.Response.ProfessorDto;
using Domain.Entities.Enums;
using System.Text.Json.Serialization;

namespace Application.Dtos.Response.StudentDto;

public class GetStudentByIdWithProfessorsDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime EnrollmentDate { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EnrollmentStatus Status { get; set; }
    public List<ProfessorsResponseDto> Professors { get; set; } = [];
}