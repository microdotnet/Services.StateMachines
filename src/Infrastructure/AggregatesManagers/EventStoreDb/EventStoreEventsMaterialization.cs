namespace MicroDotNet.Services.StateMachines.Infrastructure.AggregatesManagers.EventStoreDb
{
    using System.Threading;
    using System.Threading.Tasks;
    using EventStore.Client;

    using MicroDotNet.Services.StateMachines.Application;
    using MicroDotNet.Services.StateMachines.Application.EventsMaterialization;

    public sealed class EventStoreEventsMaterialization : IEventsMaterialization
    {
        private readonly IEventStoreClientProvider eventStoreClientProvider;

        private readonly ICheckpointManager checkpointManager;

        private readonly Func<ICheckpointManager> checkpointManagerFactory;

        public EventStoreEventsMaterialization(
            IEventStoreClientProvider eventStoreClientProvider,
            ICheckpointManager checkpointManager,
            Func<ICheckpointManager> checkpointManagerFactory)
        {
            this.eventStoreClientProvider = eventStoreClientProvider ?? throw new ArgumentNullException(nameof(eventStoreClientProvider));
            this.checkpointManager = checkpointManager ?? throw new ArgumentNullException(nameof(checkpointManager));
            this.checkpointManagerFactory = checkpointManagerFactory ?? throw new ArgumentNullException(nameof(checkpointManagerFactory));
        }

        public async Task<CreateSubscriptionResponse> CreateSubscriptionAsync(
            CreateSubscriptionRequest request,
            CancellationToken cancellationToken)
        {
            await Task.Delay(20, cancellationToken);
            return new CreateSubscriptionResponse(string.Empty);
            ////this.cancellationToken = cancellationToken;
            ////var client = this.eventStoreClientProvider.Create("EventsDB");
            ////var checkpoint = await this.GetLastCheckpoint(client, request.SubscriptionName, this.cancellationToken)
            ////    .ConfigureAwait(false);

            ////var subscription = await client.SubscribeToAllAsync(
            ////    checkpoint == null ? FromAll.Start : FromAll.After(new Position(checkpoint.Value, checkpoint.Value)),
            ////    (subscription, resolvedEvent, token) => this.HandleEvent(request.EventHandler, subscription, resolvedEvent, request.SubscriptionName, token),
            ////    this.subscriptionOptions.ResolveLinkTos,
            ////    (subscription, reason, exception) => this.HandleDrop(subscription, request.SubscriptionName, reason, exception),
            ////    this.subscriptionOptions.FilterOptions,
            ////    this.subscriptionOptions.Credentials,
            ////    this.cancellationToken).ConfigureAwait(false);
            ////return SubscribeToAllResponse.Succeeded(subscription.SubscriptionId);
        }
    }
}
