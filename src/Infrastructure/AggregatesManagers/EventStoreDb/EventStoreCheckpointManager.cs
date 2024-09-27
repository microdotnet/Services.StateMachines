namespace MicroDotNet.Services.StateMachines.Infrastructure.AggregatesManagers.EventStoreDb
{
    using System.Threading;
    using System.Threading.Tasks;
    using EventStore.Client;
    using MicroDotNet.Services.StateMachines.Application.EventsMaterialization;

    public sealed class EventStoreCheckpointManager : ICheckpointManager
    {
        private readonly EventStoreClient client;

        public EventStoreCheckpointManager(IEventStoreClientProvider eventStoreClientProvider)
        {
            if (eventStoreClientProvider == null)
            {
                throw new ArgumentNullException(nameof(eventStoreClientProvider));
            }

            this.client = eventStoreClientProvider.Create("EventsDB");
        }

        private static string GetCheckpointStreamName(string subscriptionId) => $"checkpoint_{subscriptionId}";

        public async Task<ulong?> GetLastCheckpoint(string subscriptionId, CancellationToken ct)
        {
            var streamName = GetCheckpointStreamName(subscriptionId);

            var result = this.client.ReadStreamAsync(
                Direction.Backwards,
                streamName,
                StreamPosition.End,
                1,
                cancellationToken: ct);

            if (await result.ReadState.ConfigureAwait(false) == ReadState.StreamNotFound)
            {
                return null;
            }

            ResolvedEvent? @event = await result.FirstOrDefaultAsync(ct).ConfigureAwait(false);

            return @event?.Deserialize<CheckpointStored>()?.Position;
        }

        public async Task SetLastCheckpoint(string subscriptionId, ulong position, CancellationToken ct)
        {
            var @event = new CheckpointStored(subscriptionId, position, DateTime.UtcNow);
            var eventToAppend = new[] { @event.ToJsonEventData() };
            var streamName = GetCheckpointStreamName(subscriptionId);

            try
            {
                // store new checkpoint expecting stream to exist
                await this.client.AppendToStreamAsync(
                    streamName,
                    StreamState.StreamExists,
                    eventToAppend,
                    cancellationToken: ct).ConfigureAwait(false);
            }
            catch (WrongExpectedVersionException)
            {
                // WrongExpectedVersionException means that stream did not exist
                // Set the checkpoint stream to have at most 1 event
                // using stream metadata $maxCount property
                await this.client.SetStreamMetadataAsync(
                    streamName,
                    StreamState.NoStream,
                    new StreamMetadata(1),
                    cancellationToken: ct).ConfigureAwait(false);

                // append event again expecting stream to not exist
                await this.client.AppendToStreamAsync(
                    streamName,
                    StreamState.NoStream,
                    eventToAppend,
                    cancellationToken: ct).ConfigureAwait(false);
            }
        }
    }
}
