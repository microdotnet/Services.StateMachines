namespace MicroDotNet.Services.StateMachines.Infrastructure.AggregatesManagers.EventStoreDb;

using EventStore.Client;

public interface IEventStoreClientProvider
{
    EventStoreClient? Create(string name);
}
