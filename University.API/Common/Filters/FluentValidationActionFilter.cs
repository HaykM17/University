using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace University.API.Common.Filters;

public sealed class FluentValidationActionFilter : IAsyncActionFilter
{
    private readonly IServiceProvider _serviceprovider;
    public FluentValidationActionFilter(IServiceProvider serviceprovider)
    {
        _serviceprovider = serviceprovider;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext ctx, ActionExecutionDelegate next)
    {
        foreach (var item in ctx.ActionArguments.Values)
        {
            if (item is null) 
                continue;

            var validatorType = typeof(IValidator<>).MakeGenericType(item.GetType());

            if (_serviceprovider.GetService(validatorType) is not IValidator validator) 
                continue;

            ValidationResult result = await validator.ValidateAsync(new ValidationContext<object>(item));
            if (!result.IsValid)
            {
                var errors = result.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
                ctx.Result = new BadRequestObjectResult(errors);
                return;
            }
        }

        await next();
    }
}