namespace MicroDotNet.Services.StateMachines.Infrastructure.AggregatesManagers.EventStoreDb;

using System;
using System.Threading;
using System.Threading.Tasks;

using EventStore.Client;

using MicroDotNet.Services.StateMachines.Application.AggregatesManager;
using MicroDotNet.Services.StateMachines.Domain;

public sealed class EventStoreAggregatesRepository<TAggregate> : IAggregatesRepository<TAggregate>
    where TAggregate : class, IAggregate
{
    /// <summary>
    /// <see cref="IEventStoreClientProvider"/> to use with this instance.
    /// </summary>
    private readonly IEventStoreClientProvider eventStoreClientProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventStoreStreamsProvider{TAggregate}"/> class.
    /// </summary>
    /// <param name="eventStoreClientProvider"><see cref="IEventStoreClientProvider"/> to use with this instance.</param>
    public EventStoreAggregatesRepository(
        IEventStoreClientProvider eventStoreClientProvider)
    {
        this.eventStoreClientProvider = eventStoreClientProvider ?? throw new ArgumentNullException(nameof(eventStoreClientProvider));
    }

    /// <summary>
    /// Gets <see cref="EventStoreClient"/> to use.
    /// </summary>
    private EventStoreClient Client
    {
        get
        {
            var client = this.eventStoreClientProvider.Create("WriteDB");
            if (client is null)
            {
                throw new InvalidOperationException(
                    $"Client with name 'WriteDB' not registered");
            }

            return client;
        }
    }

    /// <inheritdoc/>
    public async Task<ulong> Add(TAggregate aggregate, CancellationToken cancellationToken)
    {
        var result = await this.Client.AppendToStreamAsync(
            StreamNameMapper.ToStreamId<TAggregate>(aggregate.Id),
            StreamState.NoStream,
            GetEventsToStore(aggregate),
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

        return result.NextExpectedStreamRevision.ToUInt64();
    }

    /// <inheritdoc/>
    public Task<ulong> Delete(TAggregate aggregate, ulong? expectedRevision, CancellationToken cancellationToken)
    {
        return this.Update(aggregate, expectedRevision, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<TAggregate?> Find(Guid id, CancellationToken cancellationToken)
    {
        return await this.Client.AggregateStream<TAggregate>(
            id,
            null,
            cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<ulong> Update(TAggregate aggregate, ulong? expectedRevision, CancellationToken cancellationToken)
    {
        var eventsToAppend = GetEventsToStore(aggregate);
        var nextVersion = expectedRevision ?? aggregate.Version - (ulong)eventsToAppend.Count;

        var result = await this.Client.AppendToStreamAsync(
            StreamNameMapper.ToStreamId<TAggregate>(aggregate.Id),
            nextVersion,
            eventsToAppend,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return result.NextExpectedStreamRevision.ToUInt64();
    }

    private static List<EventData> GetEventsToStore(TAggregate aggregate)
    {
        var events = aggregate.DequeueUncommittedEvents();

        return events
            .Select(@event => @event.ToJsonEventData())
            .ToList();
    }
}
