namespace MicroDotNet.Services.StateMachines.WebApi.IntegrationTests.V1;

using System.Threading;

public partial class Tests
{
    [Fact]
    public async Task WhenMachineDefinitionIsCreatedThenCorrectResponseIsReturned()
    {
        var payload = CreateMachineDefinitionCreationPayload();
        var createMachineRequest = new HttpRequestMessage(
           HttpMethod.Post,
           "/v1/machineDefinitions");
        var requestContent = CreateJsonContent(payload);
        createMachineRequest.Content = requestContent;

        var createMachineResponse = await this.client.SendAsync(createMachineRequest, CancellationToken.None);
        createMachineResponse.IsSuccessStatusCode.Should().BeTrue($"Expected success status, but '{createMachineResponse.StatusCode}' was found.");

        createMachineResponse.Headers.Should().ContainKey("Location");
        var headerValues = createMachineResponse.Headers.GetValues("Location")
            .ToList();
        headerValues.Should().HaveCount(1);
        var location = headerValues[0];
        location.Should().Contain(payload.Code);
    }
}
