using DonationHub.Application.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace DonationHub.Application
{
    public static class ApplicationServicesRegisteration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
          
            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(typeof(IAssemblyMarker).Assembly);

            });

            services.AddValidatorsFromAssembly(typeof(IAssemblyMarker).Assembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}
