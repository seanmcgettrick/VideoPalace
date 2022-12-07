using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VideoPalace.Common.Settings;

namespace VideoPalace.Common.Extensions;

public static class MassTransitExtensions
{
    public static IServiceCollection AddMassTransitRabbitMq(
        this IServiceCollection services,
        IConfiguration configuration,
        string prefixName)
    {
        services.AddMassTransit(config =>
        {
            config.AddConsumers(Assembly.GetEntryAssembly());
            config.AddLibraryRabbitMq(configuration, prefixName);
        });

        return services;
    }

    private static void AddLibraryRabbitMq(
        this IBusRegistrationConfigurator busRegistrationConfigurator,
        IConfiguration configuration,
        string prefixName)
    {
        busRegistrationConfigurator.UsingRabbitMq((context, config) =>
        {
            var rabbitMqSettings = configuration.GetSection(nameof(RabbitMqSettings)).Get<RabbitMqSettings>();
            config.Host(rabbitMqSettings!.Host);
            config.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(prefixName, false));
            config.UseMessageRetry(rc => rc.Interval(3, TimeSpan.FromSeconds(5)));
        });
    }
}