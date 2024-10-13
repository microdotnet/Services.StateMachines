namespace MicroDotNet.Services.StateMachines.Infrastructure.AggregatesManagers.EventStoreDb;

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using EventStore.Client;

using MicroDotNet.Services.StateMachines.Application.AggregatesManager;
using MicroDotNet.Services.StateMachines.Domain;

public sealed class EventStoreAggregatesRepository<TAggregate> : IAggregatesRepository<TAggregate>
    where TAggregate : class, IAggregate
{
    private readonly IEventStoreClientProvider eventStoreClientProvider;

    private readonly EventStoreActivities activities;

    public EventStoreAggregatesRepository(
        IEventStoreClientProvider eventStoreClientProvider,
        EventStoreActivities activities)
    {
        this.eventStoreClientProvider = eventStoreClientProvider ?? throw new ArgumentNullException(nameof(eventStoreClientProvider));
        this.activities = activities;
    }

    private EventStoreClient Client
    {
        get
        {
            var client = this.eventStoreClientProvider.Create("EventsDB");
            if (client is null)
            {
                throw new InvalidOperationException(
                    $"Client with name 'EventsDB' not registered");
            }

            return client;
        }
    }

    public async Task<ulong> AddAsync(TAggregate aggregate, CancellationToken cancellationToken)
    {
        using var _ = this.activities.StartAddingAggregate(aggregate);
        var result = await this.Client.AppendToStreamAsync(
            StreamNameMapper.ToStreamId<TAggregate>(aggregate.PublicIdentifier),
            StreamState.NoStream,
            GetEventsToStore(aggregate),
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return result.NextExpectedStreamRevision.ToUInt64();
    }

    public Task<ulong> DeleteAsync(TAggregate aggregate, ulong? expectedRevision, CancellationToken cancellationToken)
    {
        return this.UpdateAsync(aggregate, expectedRevision, cancellationToken);
    }

    public async Task<TAggregate?> FindAsync(string publicIdentifier, CancellationToken cancellationToken)
    {
        return await this.Client.AggregateStream<TAggregate>(
            publicIdentifier,
            null,
            cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<ulong> UpdateAsync(TAggregate aggregate, ulong? expectedRevision, CancellationToken cancellationToken)
    {
        var eventsToAppend = GetEventsToStore(aggregate);
        var nextVersion = expectedRevision ?? aggregate.Version - (ulong)eventsToAppend.Count;

        try
        {
            var result = await this.Client.AppendToStreamAsync(
                StreamNameMapper.ToStreamId<TAggregate>(aggregate.PublicIdentifier),
                nextVersion,
                eventsToAppend,
                cancellationToken: cancellationToken)
                .ConfigureAwait(false);

        return result.NextExpectedStreamRevision.ToUInt64();
        }
        catch(Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
            throw;
        }
    }

    private static List<EventData> GetEventsToStore(TAggregate aggregate)
    {
        var events = aggregate.DequeueUncommittedEvents();

        return events
            .Select(@event => @event.ToJsonEventData())
            .ToList();
    }
}
