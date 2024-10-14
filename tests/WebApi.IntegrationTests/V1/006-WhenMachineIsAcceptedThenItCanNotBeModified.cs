namespace MicroDotNet.Services.StateMachines.WebApi.IntegrationTests.V1;

using MachineVersionTransitions = MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitionsVersionsTransitions;

partial class Tests
{
    [Fact]
    public async Task TC006_WhenMachineIsAcceptedThenItCanNotBeModified()
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
        acceptMachineResponse.IsSuccessStatusCode.Should().BeTrue();
        createNodesPayload = new Endpoints.V1.MachineDefinitionsVersionsNodes.AddNodesInput(
            [
                new Endpoints.V1.MachineDefinitionsVersionsNodes.AddNodesInput.Node("Node5"),
            ]);
        createNodesRequest = new HttpRequestMessage(
            HttpMethod.Post,
            $"{location}/nodes");
        createNodesContent = CreateJsonContent(createNodesPayload);
        createNodesRequest.Content = createNodesContent;
        var createNodesResponse = await this.client.SendAsync(createNodesRequest, CancellationToken.None);
        createNodesResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.UnprocessableEntity);
    }
}
