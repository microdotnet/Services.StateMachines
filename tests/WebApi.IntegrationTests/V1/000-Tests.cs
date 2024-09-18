namespace MicroDotNet.Services.StateMachines.WebApi.IntegrationTests.V1;

using Microsoft.AspNetCore.Mvc.Testing;

public sealed partial class Tests :
    TestClassBase,
    IClassFixture<EventStoreDbFixture>
{
    private static readonly Faker faker = new();

    private readonly CustomWebApplicationFactory<WebApiProgram> factory;

    private readonly HttpClient client;

    private readonly EventStoreDbFixture eventStoreDb;

    public Tests(
        EventStoreDbFixture eventStoreDb)
    {
        this.eventStoreDb = eventStoreDb;
        this.factory = new CustomWebApplicationFactory<WebApiProgram>(eventStoreDb);
        this.client = this.factory.CreateClient(new WebApplicationFactoryClientOptions()
        {
            AllowAutoRedirect = true,
        });
    }
}
