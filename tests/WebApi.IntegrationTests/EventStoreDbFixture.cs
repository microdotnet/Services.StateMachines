namespace MicroDotNet.Services.StateMachines.WebApi.IntegrationTests;

using System.Threading.Tasks;

using Testcontainers.EventStoreDb;

public sealed class EventStoreDbFixture : IAsyncLifetime
{
    private readonly EventStoreDbContainer container = new EventStoreDbBuilder()
        .WithImage("eventstore/eventstore:22.10.0-buster-slim")
        .Build();

    public string ConnectionString => container.GetConnectionString();

    public string ContainerId => $"{container.Id}";

    public async Task InitializeAsync()
    {
        await this.container.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await this.container.DisposeAsync();
    }
}
