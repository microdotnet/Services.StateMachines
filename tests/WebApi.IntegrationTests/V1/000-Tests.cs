namespace MicroDotNet.Services.StateMachines.WebApi.IntegrationTests.V1;

using Microsoft.AspNetCore.Mvc.Testing;

public sealed partial class Tests : TestClassBase, IClassFixture<CustomWebApplicationFactory<WebApiProgram>>
{
    private static readonly Faker faker = new();

    private readonly HttpClient client;

    public Tests(CustomWebApplicationFactory<WebApiProgram> customWebApplicationFactory)
    {
        this.client = customWebApplicationFactory.CreateClient(new WebApplicationFactoryClientOptions()
        {
            AllowAutoRedirect = true,
        });
    }
}
