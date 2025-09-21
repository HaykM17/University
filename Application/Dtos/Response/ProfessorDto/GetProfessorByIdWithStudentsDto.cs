using Application.Dtos.Response.StudentDto;
using Domain.Entities.Enums;
using System.Text.Json.Serialization;

namespace Application.Dtos.Response.ProfessorDto;

public class GetProfessorByIdWithStudentsDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime HireDate { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ProfessorRank Status { get; set; }

    public List<StudentsResponseDto> Students { get; set; } = [];
}