namespace MicroDotNet.Services.StateMachines.WebApi.IntegrationTests.V1;

using MachineVersionNodes = MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitionsVersionsNodes;

partial class Tests
{
    [Fact]
    public async Task TC004_WhenMachineVersionIsCreatedThenItCanHaveNodesAdded()
    {
        var createMachinePayload = CreateMachineCreationPayload();
        var createMachineRequest = new HttpRequestMessage(
           HttpMethod.Post,
           "/v1/machineDefinitions");
        var createMachineContent = CreateJsonContent(createMachinePayload);
        createMachineRequest.Content = createMachineContent;

        await this.client.SendAsync(createMachineRequest, CancellationToken.None);

        var createVersionRequest = new HttpRequestMessage(
            HttpMethod.Post,
            $"/v1/machineDefinitions/{createMachinePayload.Code}/versions");
        var createVersionResponse = await this.client.SendAsync(createVersionRequest, CancellationToken.None);
        var headerValues = createVersionResponse.Headers.GetValues("Location")
            .ToList();
        headerValues.Should().HaveCount(1);
        var location = headerValues[0];

        var createNodesPayload = CreateNodesCreationPayload();
        var createNodesRequest = new HttpRequestMessage(
            HttpMethod.Post,
            $"{location}/nodes");
        var createNodesContent = CreateJsonContent(createNodesPayload);
        createNodesRequest.Content = createNodesContent;
        var createNodesResponse = await this.client.SendAsync(createNodesRequest, CancellationToken.None);
        createNodesResponse.IsSuccessStatusCode.Should().BeTrue();
    }

    private static MachineVersionNodes.AddNodesInput CreateNodesCreationPayload()
    {
        return new MachineVersionNodes.AddNodesInput(
            [
                new MachineVersionNodes.AddNodesInput.Node("Node1"),
                new MachineVersionNodes.AddNodesInput.Node("Node2"),
                new MachineVersionNodes.AddNodesInput.Node("Node3"),
                new MachineVersionNodes.AddNodesInput.Node("Node4"),
            ]);
    }
}
