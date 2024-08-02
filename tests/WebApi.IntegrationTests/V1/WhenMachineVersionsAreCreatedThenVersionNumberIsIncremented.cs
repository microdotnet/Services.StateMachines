namespace MicroDotNet.Services.StateMachines.WebApi.IntegrationTests.V1;

using MachineDefinitionVersions = MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitionsVersions;

partial class Tests
{
    [Fact]
    public async Task WhenMachineVersionsAreCreatedThenVersionNumberIsIncremented()
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
        var createVersionResponsePayload = await DeserializeResponseContent<MachineDefinitionVersions.CreateOutput>(createVersionResponse);
        createVersionResponsePayload.Should().NotBeNull();
        var createSecondVersionRequest = new HttpRequestMessage(
            HttpMethod.Post,
            $"/v1/machineDefinitions/{createMachinePayload.Code}/versions");
        var createSecondVersionContent = CreateJsonContent(createVersionPayload);
        createSecondVersionRequest.Content = createSecondVersionContent;
        var createSecondVersionResponse = await this.client.SendAsync(createSecondVersionRequest, CancellationToken.None);
        var createSecondVersionResponsePayload = await DeserializeResponseContent<MachineDefinitionVersions.CreateOutput>(createSecondVersionResponse);
        createSecondVersionResponsePayload.Should().NotBeNull();
        createSecondVersionResponsePayload!.Version.Should().Be((short)(createVersionResponsePayload!.Version + 1));
    }
}
