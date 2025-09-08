using Application.Dtos.Response.ResponseStudentDto;
using Domain.Entities.Enums;
using System.Text.Json.Serialization;

namespace Application.Dtos.Response.ResponseProfessorDto;

public class GetProfessorByIdResponseDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime HireDate { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ProfessorRank Status { get; set; }
    List<GetStudentByIdResponseDto> Students { get; set; } = [];
}