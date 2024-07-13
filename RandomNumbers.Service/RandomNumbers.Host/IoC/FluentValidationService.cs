using RandomNumbers.Host.Filter;
using RandomNumbers.Host.MediatR.Handlers;
using FluentValidation.AspNetCore;

namespace RandomNumbers.Host.IoC
{
    public static class FluentValidationService
    {
        public static IServiceCollection RegisterFluentValidation(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddControllers(options =>
            {
                options.Filters.Add<AsyncValidationFilter>();
            })
            .AddFluentValidation(fv =>
            {
                fv.AutomaticValidationEnabled = false;
                fv.RegisterValidatorsFromAssemblyContaining<AuthenticateUserRequestValidator>();
            });

            //services.AddScoped<AsyncValidationFilter>();
            ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;
            ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;

            return services;
        }

    }
}
