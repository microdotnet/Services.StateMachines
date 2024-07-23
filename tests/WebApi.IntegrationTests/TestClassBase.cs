namespace MicroDotNet.Services.StateMachines.WebApi.IntegrationTests;

using System.Net.Http.Headers;
using System.Text.Json;

public abstract class TestClassBase
{
    private static readonly JsonSerializerOptions SerializationOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    protected static HttpContent CreateJsonContent(object payload)
    {
        return new StringContent(
            JsonSerializer.Serialize(payload, SerializationOptions),
            new MediaTypeHeaderValue("application/json"));
    }

    protected static async Task<TResult?> DeserializeResponseContent<TResult>(HttpResponseMessage responseMessage)
    {
        var content = await responseMessage.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TResult>(content, SerializationOptions);
    }
}
