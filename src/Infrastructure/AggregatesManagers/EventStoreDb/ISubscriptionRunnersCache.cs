namespace MicroDotNet.Services.StateMachines.Infrastructure.AggregatesManagers.EventStoreDb;

public interface ISubscriptionRunnersCache
{
    void Set(SubscriptionRunContext context);

    SubscriptionRunContext? Get(string subscriptionId);
}
