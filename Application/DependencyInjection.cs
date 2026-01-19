using System.Reflection;
using Application.Common;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // Nu hittar MediatR dina Handlers i Application-projektet
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(assembly);
                configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            // Hittar dina Validators (t.ex. RegisterUserValidator)
            services.AddValidatorsFromAssembly(assembly);

            return services;
        }
    }
}