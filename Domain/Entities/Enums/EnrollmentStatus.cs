using System.Text.Json.Serialization;

namespace Domain.Entities.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EnrollmentStatus
{
    Expelled = 0,
    Active = 1,
    Graduated = 2
}