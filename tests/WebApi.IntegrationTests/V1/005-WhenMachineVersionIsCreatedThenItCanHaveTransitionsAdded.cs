namespace MicroDotNet.Services.StateMachines.WebApi.IntegrationTests.V1;

using MachineVersionTransitions = MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitionsVersionsTransitions;

partial class Tests
{
    [Fact]
    public async Task TC005_WhenMachineVersionIsCreatedThenItCanHaveTransitionsAdded()
    {
        var createMachinePayload = CreateMachineCreationPayload();
        var createMachineRequest = new HttpRequestMessage(
           HttpMethod.Post,
           "/v1/machineDefinitions");
        var createMachineContent = CreateJsonContent(createMachinePayload);
        createMachineRequest.Content = createMachineContent;

        await this.client.SendAsync(createMachineRequest, CancellationToken.None);

        var createVersionPayload = CreateMachineDefinitionVersionCreationPayload();
        var createVersionRequest = new HttpRequestMessage(
            HttpMethod.Post,
            $"/v1/machineDefinitions/{createMachinePayload.Code}/versions");
        var createVersionContent = CreateJsonContent(createVersionPayload);
        createVersionRequest.Content = createVersionContent;
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
        var createTransitionsResponse = await this.client.SendAsync(createTransitionsRequest, CancellationToken.None);
        createTransitionsResponse.IsSuccessStatusCode.Should().BeTrue();
    }

    private static MachineVersionTransitions.AddTransitionsInput CreateTransitionsCreationPayload()
    {
        return new MachineVersionTransitions.AddTransitionsInput(
            [
                new MachineVersionTransitions.AddTransitionsInput.Transition("Node1", "Node2", "Trigger1"),
                new MachineVersionTransitions.AddTransitionsInput.Transition("Node1", "Node3", "Trigger2"),
                new MachineVersionTransitions.AddTransitionsInput.Transition("Node2", "Node4", "Trigger3"),
                new MachineVersionTransitions.AddTransitionsInput.Transition("Node1", "Node4", "Trigger4"),
            ]);
    }
}
