namespace MicroDotNet.Services.StateMachines.WebApi.IntegrationTests;

using System.Threading.Tasks;

using Testcontainers.MongoDb;

public sealed class MongoDbFixture : IAsyncLifetime
{
    private readonly MongoDbContainer container = new MongoDbBuilder()
       .WithImage("mongo")
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
