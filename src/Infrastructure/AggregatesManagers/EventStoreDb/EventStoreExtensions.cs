namespace MicroDotNet.Services.StateMachines.Infrastructure.AggregatesManagers.EventStoreDb;

using EventStore.Client;
using MicroDotNet.Services.StateMachines.Domain;

public static class EventStoreExtensions
{
    public static async Task<TAggregate?> AggregateStream<TAggregate>(
        this EventStoreClient eventStore,
        Guid id,
        ulong? fromVersion,
        CancellationToken cancellationToken)
        where TAggregate : class, IProjection
    {
        var readResult = eventStore.ReadStreamAsync(
            Direction.Forwards,
            StreamNameMapper.ToStreamId<TAggregate>(id),
            fromVersion ?? StreamPosition.Start,
            cancellationToken: cancellationToken);

        if (await readResult.ReadState.ConfigureAwait(false) == ReadState.StreamNotFound)
        {
            return null;
        }

        var aggregate = (TAggregate)Activator.CreateInstance(typeof(TAggregate), true)!;

        await foreach (var @event in readResult)
        {
            var eventData = @event.Deserialize();

            aggregate.When(eventData!);
        }

        return aggregate;
    }
}
