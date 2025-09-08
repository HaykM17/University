
using Infrastructure;
using Infrastructure.Persistence.Data;
using Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Seeding;
using University.API.Common.Filters;
using Application.Mapping;

namespace University.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        // Add services to the container.
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Custom Services
        builder.Services.AddInfrastructure()
            .AddApplication();

        //builder.Host.UseSerilog((context, configuration) =>
        //    configuration.ReadFrom.Configuration(context.Configuration));

        // DB Injections

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("UniversityConnectionString")));

        // DI Fluent Validation
        builder.Services.Configure<ApiBehaviorOptions>(o =>
        {
            o.SuppressModelStateInvalidFilter = true;
        });

        builder.Services.AddScoped<FluentValidationActionFilter>();
        builder.Services.AddControllers(o => o.Filters.Add<FluentValidationActionFilter>());


        // AutoMapper
        builder.Services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperProfiles>());


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwagger();
        }


        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        // DB Migrations
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.Migrate();
        }


        //For File(json) Seeding
        using (var scope = app.Services.CreateScope())
        {
            var seeder = scope.ServiceProvider.GetRequiredService<FileSeeder>();

            var cfg = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var profPath = cfg["Seed:ProfessorsPath"];
            var studPath = cfg["Seed:StudentsPath"];
            var profStudPath = cfg["Seed:ProfessorStudentsPath"];
            var key = cfg["Seed:JsonRunOnceKey"];

            seeder.SeedAllJsonOnceAsync(key, profPath, studPath, profStudPath).GetAwaiter().GetResult();
        }

        app.Run();
    }
}