namespace MicroDotNet.Services.StateMachines.Infrastructure.AggregatesManagers.EventStoreDb
{
    using EventStore.Client;

    public class SubscriptionRunContext
    {
        public SubscriptionRunContext(
            SubscriptionRunner runner,
            StreamSubscription subscription)
        {
            this.Runner = runner;
            this.Subscription = subscription;
        }

        public SubscriptionRunner Runner { get; }

        public StreamSubscription Subscription { get; }
    }
}
