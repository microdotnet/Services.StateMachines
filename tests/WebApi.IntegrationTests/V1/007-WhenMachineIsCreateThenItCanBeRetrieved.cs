namespace MicroDotNet.Services.StateMachines.WebApi.IntegrationTests.V1;

using MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitionsVersions;

using MachineVersionTransitions = MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitionsVersionsTransitions;

partial class Tests
{
    [Fact]
    public async Task TC007_WhenMachineIsCreateThenItCanBeRetrieved()
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
        var location = headerValues[0];

        var createNodesPayload = CreateNodesCreationPayload();
        var createNodesRequest = new HttpRequestMessage(
            HttpMethod.Post,
            $"{location}/nodes");
        var createNodesContent = CreateJsonContent(createNodesPayload);
        createNodesRequest.Content = createNodesContent;
        await this.client.SendAsync(createNodesRequest, CancellationToken.None);

        var createTransitionsPayload = CreateTransitionsCreationPayload();
        var createTransitionsRequest = new HttpRequestMessage(
            HttpMethod.Post,
            $"{location}/transitions");
        var createTransitionsContent = CreateJsonContent(createTransitionsPayload);
        createTransitionsRequest.Content = createTransitionsContent;
        await this.client.SendAsync(createTransitionsRequest, CancellationToken.None);
        var acceptMachineRequest = new HttpRequestMessage(
            HttpMethod.Post,
            $"{location}/accept");
        var acceptMachineResponse = await this.client.SendAsync(acceptMachineRequest, CancellationToken.None);
        var getMachineRequest = new HttpRequestMessage(
            HttpMethod.Get,
            location);
        var getMachineResponse = await this.client.SendAsync(getMachineRequest, CancellationToken.None);
        getMachineResponse.IsSuccessStatusCode.Should().BeTrue();
        var getMachineResponsePayload = await DeserializeResponseContent<GetOutput>(getMachineResponse);
        getMachineResponsePayload.Should().NotBeNull();
        getMachineResponsePayload!.MachineName.Should().Be(createMachinePayload.Code);
        getMachineResponsePayload.MachineVersion.Should().Be(1);
        getMachineResponsePayload.Nodes.Should().HaveCount(createNodesPayload.Nodes.Count);
        getMachineResponsePayload.Transitions.Should().HaveCount(createTransitionsPayload.Transitions.Count);
        getMachineResponsePayload.Status.Should().Be("Completed");
    }
}
