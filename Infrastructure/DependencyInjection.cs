using Application.Abstract.Repasitories;
using Application.Abstract.Services;
using Infrastructure.Repositories;
using Infrastructure.Seeding;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        // DI for Data Seeding
        services.AddScoped<FileSeeder>();
        // services.AddScoped<BogusSeeder>();

        // DI generic repository
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        // DI Services
        //services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<IProfessorService, ProfessorService>();
        return services;
    }
}