using System.Text.Json.Serialization;

namespace Domain.Entities.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ProfessorRank
{
    Assistant = 0,
    Associate = 1,
    FullProfessor = 2
}