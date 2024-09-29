namespace MicroDotNet.Services.StateMachines.WebApi.IntegrationTests.V1;

using Microsoft.AspNetCore.Mvc.Testing;

public sealed partial class Tests :
    TestClassBase,
    IClassFixture<EventStoreDbFixture>,
    IClassFixture<MongoDbFixture>
{
    private static readonly Faker faker = new();

    private readonly CustomWebApplicationFactory<WebApiProgram> factory;

    private readonly HttpClient client;

    public Tests(
        EventStoreDbFixture eventStoreDb,
        MongoDbFixture mongoDb)
    {
        this.factory = new CustomWebApplicationFactory<WebApiProgram>(
            eventStoreDb,
            mongoDb);
        this.client = this.factory.CreateClient(new WebApplicationFactoryClientOptions()
        {
            AllowAutoRedirect = true,
        });
    }
}
