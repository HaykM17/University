using Application.Common.Validation;
using Domain.Entities.Enums;
using FluentValidation;

namespace Application.Dtos.Request.ProfessorDto;

public record CreateProfessorRequestDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime HireDate { get; set; }
    public ProfessorRank Status { get; set; }
}

public class CreateProfessorRequestDtoValidator : AbstractValidator<CreateProfessorRequestDto>
{
    public CreateProfessorRequestDtoValidator()
    {
        RuleFor(dto => dto.FirstName)
            .NotNull().WithMessage("FirstName is required")
            .NotEmpty().WithMessage("FirstName can't be empty")
            .MaximumLength(50).WithMessage("FirstName length must be <= 50");

        RuleFor(dto => dto.LastName)
            .NotNull().WithMessage("v is required")
            .NotEmpty().WithMessage("LastName can't be empty")
            .MaximumLength(100).WithMessage("LastName length must be <= 100");

        RuleFor(dto => dto.Email)
           .NotNull().WithMessage("Email is required")
           .NotEmpty().WithMessage("Email can't be empty")
           .MaximumLength(255).WithMessage("Email length must be <= 255")
           .Matches(ValidationPattern.Email).WithMessage("Email format is invalid");

        RuleFor(dto => dto.HireDate)
           .LessThanOrEqualTo(DateTime.Today).WithMessage("HireDate can't be in the future");

        RuleFor(dto => dto.Status)
            .IsInEnum().WithMessage("Invalid ProfessorRank value");
    }
}