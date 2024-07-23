namespace MicroDotNet.Services.StateMachines.WebApi.IntegrationTests;

using System.Net.Http.Headers;
using System.Text.Json;

public abstract class TestClassBase
{
    protected static HttpContent CreateJsonContent(object payload)
    {
        return new StringContent(
            JsonSerializer.Serialize(payload),
            new MediaTypeHeaderValue("application/json"));
    }
}
