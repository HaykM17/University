using FluentValidation;
using FluentValidation.Validators;

namespace Application.Dtos.Request.StudentDto;

public record UpdateStudentFirstNameAndLastNameRequestDto(
    string FirstName,
    string LastName
);

public class UpdateStudentFirstNameAndLastNameRequestDtoValidator : AbstractValidator<UpdateStudentFirstNameAndLastNameRequestDto>
{
    public UpdateStudentFirstNameAndLastNameRequestDtoValidator()
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