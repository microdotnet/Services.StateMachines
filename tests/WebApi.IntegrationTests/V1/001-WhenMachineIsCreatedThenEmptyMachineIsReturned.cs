namespace MicroDotNet.Services.StateMachines.WebApi.IntegrationTests.V1;

using MachineDefinitions = MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitions;

partial class Tests
{
    [Fact]
    public async Task TC001_WhenMachineIsCreatedThenEmptyMachineIsReturned()
    {
        var payload = CreateMachineCreationPayload();
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
        
    private static MachineDefinitions.CreateInput CreateMachineCreationPayload()
    {
        return new MachineDefinitions.CreateInput(
            $"MachineDefinition_{DateTime.Now.Ticks}",
            faker.Commerce.ProductName(),
            faker.Commerce.ProductDescription());
    }
}
