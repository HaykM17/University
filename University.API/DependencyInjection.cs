using Microsoft.AspNetCore.Mvc;
using University.API.Common.Filters;

namespace University.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebApi(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;

            // DI Fluent Validation
            services.Configure<ApiBehaviorOptions>(o =>
            {
                o.SuppressModelStateInvalidFilter = true;
            });

            services.AddScoped<FluentValidationActionFilter>();
            services.AddControllers(o => o.Filters.Add<FluentValidationActionFilter>());

            return services;
        }
    }
}