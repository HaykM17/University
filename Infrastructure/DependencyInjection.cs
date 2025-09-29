using Application.Repositories.GenericRepository;
using Infrastructure.Persistence.Data;
using Infrastructure.Repositories.Concrete;
using Infrastructure.Repositories.GenericRepository;
using Infrastructure.Repositories.TokenRepository;
using Infrastructure.Seeding;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<ITokenRepository<string, IdentityUser, List<string>>, TokenRepository>();

        // University Db connect
        services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(config.GetConnectionString("UniversityConnectionString")));


        // University Authentication Db connect
        services.AddDbContext<UniversityAuthDbContext>(options =>
        options.UseSqlServer(config.GetConnectionString("UniversityAuthConnectionString")));


        // Identity DI
        services.AddIdentityCore<IdentityUser>()
            .AddRoles<IdentityRole>()
            .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("University")
            .AddEntityFrameworkStores<UniversityAuthDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;
        });

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            options.TokenValidationParameters = new TokenValidationParameters
            {
                AuthenticationType = "Jwt",
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = config["Jwt:Issuer"],
                ValidAudience = config["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!)),
                ClockSkew = TimeSpan.FromMinutes(3)
            });

        return services;
    }
}