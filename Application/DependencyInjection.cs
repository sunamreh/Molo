using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using Molo.Application.Common.Behaviours;
using Molo.Application.Molo.Subscription.Commands;
using System.Reflection;

namespace Molo.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            
            return services;
        }
    }
}
