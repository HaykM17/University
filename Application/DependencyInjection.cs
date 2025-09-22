using Application.Services.Abstract;
using Application.Services.Concrete;
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

        // DI Services
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<IProfessorService, ProfessorService>();

        return services;
    }
}