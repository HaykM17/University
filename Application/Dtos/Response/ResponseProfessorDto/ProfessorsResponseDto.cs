using Domain.Entities.Enums;
using System.Text.Json.Serialization;

namespace Application.Dtos.Response.ResponseProfessorDto;

public class ProfessorsResponseDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime HireDate { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ProfessorRank Status { get; set; }
    public DateTime CreatedAt { get; set; }
}