namespace MicroDotNet.Services.StateMachines.Infrastructure.AggregatesManagers.EventStoreDb;

using MicroDotNet.Services.StateMachines.Application;
using MicroDotNet.Services.StateMachines.Application.AggregatesManager;
using MicroDotNet.Services.StateMachines.Infrastructure.ReadModel.Mongo;

using Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
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

        return services;
    }
}
