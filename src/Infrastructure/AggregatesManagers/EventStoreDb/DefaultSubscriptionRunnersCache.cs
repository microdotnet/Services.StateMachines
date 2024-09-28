namespace MicroDotNet.Services.StateMachines.Infrastructure.AggregatesManagers.EventStoreDb;

public sealed class DefaultSubscriptionRunnersCache : ISubscriptionRunnersCache
{
    private readonly Dictionary<string, SubscriptionRunContext> subscriptions = [];

    public SubscriptionRunContext? Get(string subscriptionId)
    {
        if (!this.subscriptions.TryGetValue(subscriptionId, out var result))
        {
            return null;
        }

        return result;
    }

    public void Set(SubscriptionRunContext context)
    {
        this.subscriptions.Add(context.Subscription.SubscriptionId, context);
    }
}
