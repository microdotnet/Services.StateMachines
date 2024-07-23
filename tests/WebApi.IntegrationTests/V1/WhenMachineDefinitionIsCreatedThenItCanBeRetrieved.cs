namespace MicroDotNet.Services.StateMachines.WebApi.IntegrationTests.V1;

using System.Threading;

using MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitions;

public partial class TestClass
{
    [Fact]
    public async Task WhenMachineDefinitionIsCreatedThenItCanBeRetrieved()
    {
        var payload = CreateMachineDefinitionCreationPayload();
        var createMachineRequest = new HttpRequestMessage(
           HttpMethod.Post,
           "/v1/machineDefinitions");
        var requestContent = CreateJsonContent(payload);
        createMachineRequest.Content = requestContent;

        var createMachineResponse = await this.client.SendAsync(createMachineRequest, CancellationToken.None);
        var headerValues = createMachineResponse.Headers.GetValues("Location")
            .ToList();
        headerValues.Should().HaveCount(1);
        var location = headerValues[0];
        var getMachineResponseMessage = await this.client.GetAsync(location);
        getMachineResponseMessage.IsSuccessStatusCode.Should().BeTrue("Expected successful retrieval");
        var getMachineResponse = await DeserializeResponseContent<GetResult>(getMachineResponseMessage);
        getMachineResponse.Should().NotBeNull();
        getMachineResponse!.MachineCode.Should().Be(payload.Code);
        getMachineResponse.MachineVersion.Should().Be(payload.Version);
    }
}
