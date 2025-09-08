namespace Domain.Entities.Base;

public abstract class SoftDeletableEntity : EntityBase
{
    public bool IsDeleted { get; set; }
}