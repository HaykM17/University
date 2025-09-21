using Application.Common.Validation;
using Domain.Entities.Enums;
using FluentValidation;
using FluentValidation.Validators;
using System.Text.Json.Serialization;

namespace Application.Dtos.Request.ProfessorDto;

public record BulkUpdateProfessorRequestDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime HireDate { get; set; }
    public ProfessorRank Status { get; set; }

    [JsonIgnore]
    public DateTime? UpdatedAt { get; set; }
}

public class BulkUpdateProfessorRequestDtoValidator : AbstractValidator<BulkUpdateProfessorRequestDto>
{
    public BulkUpdateProfessorRequestDtoValidator()
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

        RuleFor(x => x.HireDate)
           .LessThanOrEqualTo(DateTime.Today).WithMessage("EnrollmentDate can't be in the future");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid EnrollmentStatus value");
    }
}