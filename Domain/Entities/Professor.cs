using Domain.Entities.Base;
using Domain.Entities.Enums;

namespace Domain.Entities;

public class Professor : SoftDeletableEntity
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime HireDate { get; set; }
    public ProfessorRank Status { get; set; }
    public List<ProfessorStudent> ProfessorStudents { get; set; } = [];
}