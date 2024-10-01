namespace MicroDotNet.Services.StateMachines.WebApi.IntegrationTests.V1;

using System.Net;

using MachineDefinitions = MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitions;

partial class Tests
{
    [Fact]
    public async Task TC008_WhenMachineIsCreatedThenItCanBeRetrieved()
    {
        var payload = CreateMachineCreationPayload();
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
        const int GetMaxRetries = 5;
        var attemptCounter = 0;
        while (true)
        {
            await Task.Delay(1000 * attemptCounter);

            var getMachineRequest = new HttpRequestMessage(
                HttpMethod.Get,
                location);
            var getMachineResponse = await this.client.SendAsync(getMachineRequest, CancellationToken.None);
            if (getMachineResponse.StatusCode == HttpStatusCode.OK)
            {
                var getMachineResponsePayload = await DeserializeResponseContent<MachineDefinitions.GetOutput>(getMachineResponse);
                getMachineResponsePayload.Should().NotBeNull();
                getMachineResponsePayload!.Code.Should().Be(payload.Code);
                getMachineResponsePayload!.Name.Should().Be(payload.Name);
                getMachineResponsePayload!.Description.Should().Be(payload.Description);
            }

            attemptCounter++;
            if (attemptCounter == GetMaxRetries)
            {
                Assert.Fail($"Waited {GetMaxRetries} attempts, but machine was not retrieved");
                break;
            }
        }
    }
}