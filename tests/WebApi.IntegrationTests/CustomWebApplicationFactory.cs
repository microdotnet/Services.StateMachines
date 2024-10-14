namespace MicroDotNet.Services.StateMachines.WebApi.IntegrationTests;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

public sealed class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
    where TProgram : class
{
    private readonly EventStoreDbFixture eventStoreDb;

    private readonly MongoDbFixture mongoDb;

    public CustomWebApplicationFactory(
        EventStoreDbFixture eventStoreDb,
        MongoDbFixture mongoDb)
    {
        this.eventStoreDb = eventStoreDb;
        this.mongoDb = mongoDb;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.ConfigureTestServices(services =>
        {
            // Configure services for tests.
        });
        builder.ConfigureAppConfiguration((ctx, bld) =>
        {
            var configuration = new Dictionary<string, string?>()
            {
                ["ConnectionStrings:EventsDB"] = this.eventStoreDb.ConnectionString,
                ["ConnectionStrings:ReadDB"] = this.mongoDb.ConnectionString,
            };
            bld.AddInMemoryCollection(configuration);
        });
            
        builder.UseEnvironment("Development");
    }
}
