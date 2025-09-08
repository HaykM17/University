using Application.Mapping;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        // DI Validators
        services.AddValidatorsFromAssembly(assembly);


        // DI AutoMapper
        services.AddAutoMapper(cfg => cfg.AddMaps(assembly));

        return services;
    }
}