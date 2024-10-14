namespace MicroDotNet.Services.StateMachines.WebApi.IntegrationTests.V1;

using MachineDefinitionVersions = MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitionsVersions;

partial class Tests
{
    [Fact]
    public async Task TC002_WhenMachineVersionIsCreatedThenCorrectResponseIsReturned()
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
        createVersionResponse.IsSuccessStatusCode.Should().BeTrue($"Expected success status, but '{createVersionResponse.StatusCode}' was found.");

        createVersionResponse.Headers.Should().ContainKey("Location");
        var headerValues = createVersionResponse.Headers.GetValues("Location")
            .ToList();
        headerValues.Should().HaveCount(1);
        var location = headerValues[0];
        location.Should().Contain(createMachinePayload.Code);
    }
}
