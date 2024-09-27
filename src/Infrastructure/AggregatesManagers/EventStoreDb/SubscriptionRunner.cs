namespace MicroDotNet.Services.StateMachines.Infrastructure.AggregatesManagers.EventStoreDb
{
    using EventStore.Client;
    using Grpc.Core;

    using MicroDotNet.Services.StateMachines.Application.EventsMaterialization;

    using Microsoft.Extensions.Logging;

    public class SubscriptionRunner
    {
        private static readonly SubscriptionOptions subscriptionOptions = new();

        private readonly ICheckpointManager checkpointManager;

        private readonly EventTypeMapper eventTypeMapper = EventTypeMapper.Instance;

        private readonly object resubscribeLock = new();

        private readonly ILogger logger;

        public SubscriptionRunner(
            ICheckpointManager checkpointManager,
            ILogger<SubscriptionRunner> logger)
        {
            this.checkpointManager = checkpointManager;
            this.logger = logger;
        }

        internal static async Task<SubscriptionRunContext> Create(
            Func<SubscriptionRunner> instanceFactory,
            CreateSubscriptionRequest request,
            EventStoreClient client,
            CancellationToken cancellationToken)
        {
            var runner = instanceFactory();
            var checkpoint = await runner.checkpointManager.GetLastCheckpoint(request.SubscriptionName, cancellationToken)
                .ConfigureAwait(false);

            var subscription = await client.SubscribeToAllAsync(
                checkpoint == null ? FromAll.Start : FromAll.After(new Position(checkpoint.Value, checkpoint.Value)),
                (subscription, resolvedEvent, token) => runner.HandleEvent(request.Handler, subscription, resolvedEvent, request.SubscriptionName, token),
                subscriptionOptions.ResolveLinkTos,
                (subscription, reason, exception) => runner.HandleDrop(subscription, request.SubscriptionName, reason, exception),
                subscriptionOptions.FilterOptions,
                subscriptionOptions.Credentials,
                cancellationToken)
                .ConfigureAwait(false);
            return new SubscriptionRunContext(runner, subscription);
        }

        private static IEventEnvelope? ToEventEnvelope(ResolvedEvent resolvedEvent)
        {
            var eventData = resolvedEvent.Deserialize();

            if (eventData == null)
            {
                return null;
            }

            var metaData = new EventMetadata(
                resolvedEvent.Event.EventId.ToString(),
                resolvedEvent.Event.EventNumber.ToUInt64(),
                resolvedEvent.Event.Position.CommitPosition);

            return EventEnvelopeFactory.From(eventData, metaData);
        }

        private async Task HandleEvent(
            CreateSubscriptionRequest.StreamEventHandler eventHandler,
            StreamSubscription subscription,
            ResolvedEvent resolvedEvent,
            string subscriptionName,
            CancellationToken token)
        {
            try
            {
                if (this.IsEventWithEmptyData(resolvedEvent) || this.IsCheckpointEvent(resolvedEvent))
                {
                    return;
                }

                var eventEnvelope = ToEventEnvelope(resolvedEvent);

                if (eventEnvelope == null)
                {
                    // That can happen if we're sharing database between modules.
                    // If we're subscribing to all and not filtering out events from other modules,
                    // then we might get events that are from other module and we might not be able to deserialize them.
                    // In that case it's safe to ignore deserialization error.
                    // You may add more sophisticated logic checking if it should be ignored or not.
                    this.logger.LogWarning("Couldn't deserialize event with id: {EventId}", resolvedEvent.Event.EventId);

                    throw new InvalidOperationException(
                        $"Unable to deserialize event {resolvedEvent.Event.EventType} with id: {resolvedEvent.Event.EventId}");
                }

                await eventHandler(eventEnvelope, token)
                    .ConfigureAwait(false);

                await this.checkpointManager.SetLastCheckpoint(subscriptionName, resolvedEvent.Event.Position.CommitPosition, token)
                    .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                this.logger.LogError(
                    "[{SubscriptionName}] - Error consuming message: {ExceptionMessage}{ExceptionStackTrace}",
                    subscriptionName,
                    e.Message,
                    e.StackTrace);

                // if you're fine with dropping some events instead of stopping subscription
                // then you can add some logic if error should be ignored
                throw;
            }
        }

        private void HandleDrop(StreamSubscription subscription, string subscriptionName, SubscriptionDroppedReason reason, Exception? exception)
        {
            if (exception is RpcException { StatusCode: StatusCode.Cancelled })
            {
                this.logger.LogWarning(
                    "Subscription '{SubscriptionId}' dropped by client",
                    subscriptionName);

                return;
            }

            this.logger.LogError(
                exception,
                "Subscription to all '{SubscriptionId}' dropped with '{StatusCode}' and '{Reason}'",
                subscriptionName,
                (exception as RpcException)?.StatusCode ?? StatusCode.Unknown,
                reason);

            this.Resubscribe(subscriptionName, CancellationToken.None);
        }

        private void Resubscribe(string subscriptionName, CancellationToken cancellationToken)
        {
            // You may consider adding a max resubscribe count if you want to fail process
            // instead of retrying until database is up
            while (true)
            {
                var resubscribed = false;
                try
                {
                    Monitor.Enter(this.resubscribeLock);

                    // No synchronization context is needed to disable synchronization context.
                    // That enables running asynchronous method not causing deadlocks.
                    // As this is a background process then we don't need to have async context here.
                    using (NoSynchronizationContextScope.Enter())
                    {
                        this.checkpointManager.GetLastCheckpoint(
                            subscriptionName,
                            cancellationToken)
                            .Wait(cancellationToken);
                    }

                    resubscribed = true;
                }
                catch (Exception exception)
                {
                    this.logger.LogWarning(
                        exception,
                        "Failed to resubscribe to all '{SubscriptionId}' dropped with '{ExceptionMessage}{ExceptionStackTrace}'",
                        subscriptionName,
                        exception.Message,
                        exception.StackTrace);
                }
                finally
                {
                    Monitor.Exit(this.resubscribeLock);
                }

                if (resubscribed)
                {
                    break;
                }

                // Sleep between reconnections to not flood the database or not kill the CPU with infinite loop
                // Randomness added to reduce the chance of multiple subscriptions trying to reconnect at the same time
                Thread.Sleep(1000 + new Random((int)DateTime.UtcNow.Ticks).Next(1000));
            }
        }

        private bool IsEventWithEmptyData(ResolvedEvent resolvedEvent)
        {
            if (resolvedEvent.Event.Data.Length != 0)
            {
                return false;
            }

            this.logger.LogWarning("Event without data received");
            return true;
        }

        private bool IsCheckpointEvent(ResolvedEvent resolvedEvent)
        {
            if (resolvedEvent.Event.EventType != this.eventTypeMapper.ToName<CheckpointStored>())
            {
                return false;
            }

            this.logger.LogInformation("Checkpoint event - ignoring");
            return true;
        }

        private static class NoSynchronizationContextScope
        {
            public static Disposable Enter()
            {
                var context = SynchronizationContext.Current!;
                SynchronizationContext.SetSynchronizationContext(null);
                return new Disposable(context);
            }

            public readonly struct Disposable : IDisposable
            {
                private readonly SynchronizationContext synchronizationContext;

                public Disposable(SynchronizationContext synchronizationContext)
                {
                    this.synchronizationContext = synchronizationContext;
                }

                public void Dispose() =>
                    SynchronizationContext.SetSynchronizationContext(this.synchronizationContext);
            }
        }
    }
}
