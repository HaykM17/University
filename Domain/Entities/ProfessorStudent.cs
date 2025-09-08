using Domain.Entities.Base;

namespace Domain.Entities;

public class ProfessorStudent : EntityBase
{
    public int ProfessorId { get; set; }
    public Professor Professor { get; set; } = null!;

    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;
}