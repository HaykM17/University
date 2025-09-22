using Application.Repositories;
using Infrastructure.Repositories;
using Infrastructure.Seeding;
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

        
        return services;
    }
}