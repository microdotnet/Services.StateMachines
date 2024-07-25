namespace MicroDotNet.Services.StateMachines.WebApi.IntegrationTests.V1;

using System.Net;
using System.Threading;

using MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitions;

public partial class Tests
{
    [Fact]
    public async Task WhenMachineIsConfirmedItCannotBeUpdated()
    {
        var createMachinePayload = CreateMachineDefinitionCreationPayload();
        var createMachineRequest = new HttpRequestMessage(
           HttpMethod.Post,
           "/v1/machineDefinitions");
        var createMachineContent = CreateJsonContent(createMachinePayload);
        createMachineRequest.Content = createMachineContent;
        var createMachineResponse = await this.client.SendAsync(createMachineRequest, CancellationToken.None);
        var headerValues = createMachineResponse.Headers.GetValues("Location")
            .ToList();
        var location = headerValues[0];
        var updateMachineRequest = new HttpRequestMessage(
            HttpMethod.Put,
            location);
        var confirmMachineRequest = new HttpRequestMessage(
            HttpMethod.Post,
            $"/v1/machineDefinitions/{createMachinePayload.Code}/{createMachinePayload.Version}/confirm");
        var confirmMachineResponse = await this.client.SendAsync(confirmMachineRequest, CancellationToken.None);
        var confirmMachineResponseContent = await confirmMachineResponse.Content.ReadAsStringAsync();
        Console.WriteLine(confirmMachineResponseContent);
        confirmMachineResponse.IsSuccessStatusCode.Should().BeTrue();
        var updateMachinePayload = CreateMachineDefinitionModificationPayload();
        var updateMachineContent = CreateJsonContent(updateMachinePayload);
        updateMachineRequest.Content = updateMachineContent;
        var updateMachineResponse = await this.client.SendAsync(updateMachineRequest, CancellationToken.None);
        updateMachineResponse.StatusCode.Should()
            .Be(HttpStatusCode.UnprocessableEntity,
            $"Expected '{HttpStatusCode.UnprocessableEntity}' status, but '{updateMachineResponse.StatusCode}' was found.");
    }
}
