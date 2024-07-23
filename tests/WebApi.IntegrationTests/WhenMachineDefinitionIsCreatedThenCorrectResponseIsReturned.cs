namespace MicroDotNet.Services.StateMachines.WebApi.IntegrationTests;

using MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitions;

using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;

public partial class V1Tests
{
    [Fact]
    public async Task WhenMachineDefinitionIsCreatedThenCorrectResponseIsReturned()
    {
        var payload = new PostPayload(
             $"TestMachine_{DateTime.Now.Ticks}",
             1,
             [
                 new MachineNode("Node1"),
                 new MachineNode("Node2"),
                 new MachineNode("Node3"),
             ],
             [
                 new NodeTransition("Node1", "Node2"),
                 new NodeTransition("Node2", "Node3")
             ]);
        var createMachineRequest = new HttpRequestMessage(
           HttpMethod.Post,
           "/v1/machineDefinitions");
        var requestContent = new StringContent(
            JsonSerializer.Serialize(payload),
            new MediaTypeHeaderValue("application/json"));
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
