using MediatR.NotificationPublishers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using UserIPAnalytics.Application.Abstractions;

namespace UserIPAnalytics.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR();


            return services;
        }

        private static IServiceCollection AddMediatR(this IServiceCollection services)
        {
            services.AddMediatR(config =>
            {
                Assembly[] handlersAssemblies = new[]
                {
            typeof(INotificationHandlerEvent).GetTypeInfo().Assembly,
            };
                config.RegisterServicesFromAssemblies(handlersAssemblies);
                config.NotificationPublisher = new TaskWhenAllPublisher();
            });

            return services;
        }
    }
}
