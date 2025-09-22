using Application.Repositories;
using Infrastructure.Persistence.Data;
using Infrastructure.Repositories;
using Infrastructure.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        // DI for Data Seeding
        services.AddScoped<FileSeeder>();
        // services.AddScoped<BogusSeeder>();

        // DI generic repository
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(config.GetConnectionString("UniversityConnectionString")));

        return services;
    }
}