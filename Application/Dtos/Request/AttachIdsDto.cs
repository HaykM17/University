using FluentValidation;

namespace Application.Dtos.Request;

public record AttachIdsDto
{
    public int Id {  get; set; }
    public List<int> Ids { get; set; } = [];
}

public class AttachIdsDtoValidator : AbstractValidator<AttachIdsDto>
{
    public AttachIdsDtoValidator()
    {
        RuleFor(x => x.Id)
           .GreaterThanOrEqualTo(1)
           .WithMessage("Id must be greater than or equal to 1.");

        RuleFor(x => x.Ids)
            .NotNull().WithMessage("Ids list cannot be null.")
            .Must(list => list.Count > 0).WithMessage("Ids list cannot be empty.");

        RuleForEach(x => x.Ids)
            .GreaterThan(0)
            .WithMessage("Each Id in the list must be greater than 0.");
    }
}