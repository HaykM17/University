using Application.Mapping;
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

        services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperProfiles>());

        // DI Services
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<IProfessorService, ProfessorService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}