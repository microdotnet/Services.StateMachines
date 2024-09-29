namespace MicroDotNet.Services.StateMachines.Infrastructure.AggregatesManagers.EventStoreDb
{
    using EventStore.Client;

    internal sealed class SubscriptionOptions
    {
        public SubscriptionFilterOptions FilterOptions { get; set; } = new(EventStore.Client.EventTypeFilter.ExcludeSystemEvents());

        public Action<EventStoreClientOperationOptions>? ConfigureOperation { get; set; }

        public UserCredentials? Credentials { get; set; }

        public bool ResolveLinkTos { get; set; }

        public bool IgnoreDeserializationErrors { get; set; } = true;
    }
}
