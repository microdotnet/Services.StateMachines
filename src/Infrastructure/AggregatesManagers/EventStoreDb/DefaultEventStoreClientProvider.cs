namespace MicroDotNet.Services.StateMachines.Infrastructure.AggregatesManagers.EventStoreDb;

using EventStore.Client;

using Microsoft.Extensions.Configuration;

public sealed class DefaultEventStoreClientProvider : IEventStoreClientProvider
{
    private readonly Dictionary<string, EventStoreClient> clients = [];

    private readonly SemaphoreSlim createLock = new (1, 1);

    private readonly IConfiguration configuration;

    public DefaultEventStoreClientProvider(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public EventStoreClient? Create(string name)
    {
        if (this.clients.TryGetValue(name, out var value))
        {
            return value;
        }

        this.createLock.Wait();
        try
        {
            if (this.clients.TryGetValue(name, out value))
            {
                return value;
            }

            var settings = CreateClientSettings(this.configuration, name);
            var client = new EventStoreClient(settings);
            this.clients.Add(name, client);
            return client;
        }
        finally
        {
            this.createLock.Release();
        }
    }

    private static EventStoreClientSettings CreateClientSettings(
        IConfiguration configuration,
        string connectionStringName)
    {
        if (string.IsNullOrWhiteSpace(connectionStringName))
        {
            throw new ArgumentNullException(
                nameof(connectionStringName),
                "Connection string name cannot be empty.");
        }

        var connectionString = configuration.GetConnectionString(connectionStringName);
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException(
                $"Connection string '{connectionStringName}' cannot be empty.",
                nameof(connectionStringName));
        }

        return EventStoreClientSettings.Create(connectionString);
    }
}
