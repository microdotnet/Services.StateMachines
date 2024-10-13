namespace MicroDotNet.Services.StateMachines.Infrastructure.AggregatesManagers.EventStoreDb;

using MicroDotNet.Services.StateMachines.Application;
using MicroDotNet.Services.StateMachines.Application.AggregatesManager;
using MicroDotNet.Services.StateMachines.Domain.MachineDetails;
using MicroDotNet.Services.StateMachines.Infrastructure.ReadModel.Mongo;

using Microsoft.Extensions.DependencyInjection;

using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

public static class StartupExtensions
{
    public static IServiceCollection AddEventStoreDb(
        this IServiceCollection services)
    {
        services.AddSingleton<IEventStoreClientProvider, DefaultEventStoreClientProvider>();
        return services;
    }

    public static IServiceCollection StoreMachinesInEventStoreDb(
        this IServiceCollection services)
    {
        services.AddEventStoreDb();
        services.AddMongoReadModel();

        services.AddScoped(typeof(IAggregatesRepository<>), typeof(EventStoreAggregatesRepository<>));
        services.AddScoped<IEventsMaterialization, EventStoreEventsMaterialization>();
        services.AddSingleton<ICheckpointManager, EventStoreCheckpointManager>();
        services.AddSingleton<ISubscriptionRunnersCache, DefaultSubscriptionRunnersCache>();
        services.AddTransient<SubscriptionRunner>();
        services.AddSingleton<EventStoreActivities>();

        return services;
    }

    public static TracerProviderBuilder AddEventStoreDbTraces(
        this TracerProviderBuilder builder)
    {
        builder
            .AddSource(EventStoreActivities.ActivityName);
        return builder;
    }

    public static MeterProviderBuilder AddEventStoreDbMeters(
        this MeterProviderBuilder builder)
    {
        return builder;
    }
}
