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

        private readonly ISubscriptionRunnersCache subscriptionRunnersCache;

        private readonly Func<SubscriptionRunner> subscriptionRunnerFactory;

        public EventStoreEventsMaterialization(
            IEventStoreClientProvider eventStoreClientProvider,
            ICheckpointManager checkpointManager,
            Func<SubscriptionRunner> checkpointManagerFactory,
            ISubscriptionRunnersCache subscriptionRunnersCache)
        {
            this.eventStoreClientProvider = eventStoreClientProvider ?? throw new ArgumentNullException(nameof(eventStoreClientProvider));
            this.checkpointManager = checkpointManager ?? throw new ArgumentNullException(nameof(checkpointManager));
            this.subscriptionRunnerFactory = checkpointManagerFactory ?? throw new ArgumentNullException(nameof(checkpointManagerFactory));
            this.subscriptionRunnersCache = subscriptionRunnersCache ?? throw new ArgumentNullException(nameof(subscriptionRunnersCache));
        }

        public async Task<CreateSubscriptionResponse> CreateSubscriptionAsync(
            CreateSubscriptionRequest request,
            CancellationToken stopProcessingToken)
        {
            var client = this.eventStoreClientProvider.Create("EventsDB");
            var runner = await SubscriptionRunner.Create(this.subscriptionRunnerFactory, request, client, stopProcessingToken)
                .ConfigureAwait(false);
            this.subscriptionRunnersCache.Set(runner);
            return new CreateSubscriptionResponse(runner.Subscription.SubscriptionId);
        }
    }
}
