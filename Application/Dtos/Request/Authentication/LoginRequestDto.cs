using Application.Common.Validation;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Authentication;

public record LoginRequestDto
{
    [DataType(DataType.EmailAddress)]
    public string UserName { get; set; } = null!;

    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}
public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
{
    public LoginRequestDtoValidator()
    {
        RuleFor(dto => dto.UserName)
         .NotNull().WithMessage("UserName is required")
         .NotEmpty().WithMessage("UserName can't be empty")
         .EmailAddress().WithMessage("UserName must be valid email")
         .Matches(ValidationPattern.Email).WithMessage("UserName must be valid email");

        RuleFor(dto => dto.Password)
         .NotNull().WithMessage("Password is required")
         .NotEmpty().WithMessage("Password can't be empty")
         .MinimumLength(6).WithMessage("Password must be exactly 6 characters");
    }
}