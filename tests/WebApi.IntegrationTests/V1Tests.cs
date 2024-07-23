namespace MicroDotNet.Services.StateMachines.WebApi.IntegrationTests;

using Microsoft.AspNetCore.Mvc.Testing;

public partial class V1Tests : IClassFixture<CustomWebApplicationFactory<WebApiProgram>>
{
    private readonly CustomWebApplicationFactory<WebApiProgram> customWebApplicationFactory;

    private readonly HttpClient client;

    public V1Tests(CustomWebApplicationFactory<WebApiProgram> customWebApplicationFactory)
    {
        this.customWebApplicationFactory = customWebApplicationFactory;
        client = customWebApplicationFactory.CreateClient(new WebApplicationFactoryClientOptions()
        {
            AllowAutoRedirect = true,
        });
    }
}
