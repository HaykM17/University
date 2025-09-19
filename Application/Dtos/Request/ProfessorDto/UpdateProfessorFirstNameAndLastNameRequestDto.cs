using FluentValidation;

namespace Application.Dtos.Request.ProfessorDto;

public record UpdateProfessorFirstNameAndLastNameRequestDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
}

public class UpdateProfessorFirstNameAndLastNameRequestDtoValidator : AbstractValidator<UpdateProfessorFirstNameAndLastNameRequestDto>
{
    public UpdateProfessorFirstNameAndLastNameRequestDtoValidator()
    {
        RuleFor(dto => dto.FirstName)
          .NotNull().WithMessage("FirstName is required")
          .NotEmpty().WithMessage("FirstName can't be empty")
          .MaximumLength(50).WithMessage("FirstName length must be <= 50");

        RuleFor(dto => dto.LastName)
            .NotNull().WithMessage("LastName is required")
            .NotEmpty().WithMessage("LastName can't be empty")
            .MaximumLength(100).WithMessage("LastName length must be <= 100");
    }
}