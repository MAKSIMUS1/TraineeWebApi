using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using WebApiTrainingProject.Middlewares;

namespace WebApiTrainingProject.Utils
{
    public static class FluentValidationStartup
    {
        public static IServiceCollection AddFluentValidationStartup(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssembly(typeof(FluentValidationStartup).Assembly);

            return services;
        }
    }
}
