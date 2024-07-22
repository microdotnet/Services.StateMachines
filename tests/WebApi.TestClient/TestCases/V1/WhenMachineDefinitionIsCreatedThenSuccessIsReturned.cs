namespace MicroDotNet.Services.StateMachines.WebApi.TestClient.TestCases.V1;

using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitions;

public sealed class WhenMachineDefinitionIsCreatedThenSuccessIsReturned : StateMachinesTestCaseBase
{
    public WhenMachineDefinitionIsCreatedThenSuccessIsReturned(IOutputWriter outputWriter)
        : base(outputWriter)
    {
    }

    protected override async Task<TestRunResult> RunAsync(StateMachinesTestRunContext context, CancellationToken cancellationToken)
    {
        var client = new HttpClient()
        {
            BaseAddress = new(context.RootUrl),
        };
        var payload = new PostPayload(
            $"TestMachine_{DateTime.Now.Ticks}",
            1,
            [
                new MachineNode("Node1"),
                new MachineNode("Node2"),
                new MachineNode("Node3"),
            ],
            [
                new NodeTransition("Node1", "Node2"),
                new NodeTransition("Node2", "Node3")
            ]);
        var createMachineRequest = new HttpRequestMessage(
           HttpMethod.Post,
           "/v1/machineDefinitions");
        var requestContent = new StringContent(
            JsonSerializer.Serialize(payload),
            new MediaTypeHeaderValue("application/json"));
        createMachineRequest.Content = requestContent;

        var createMachineResponse = await client.SendAsync(createMachineRequest, cancellationToken)
            .ConfigureAwait(false);
        if (!createMachineResponse.IsSuccessStatusCode)
        {
            var message = $"Expected success status, but '{createMachineResponse.StatusCode}' was found.";
            await this.OutputWriter.WriteError(message)
                .ConfigureAwait(false);
            return new(TestRunResult.Result.Failure);
        }

        if (!createMachineResponse.Headers.Contains("Location"))
        {
            await this.OutputWriter.WriteError(
                "Expected 'Location' header, but was not found");
            return new(TestRunResult.Result.Failure);
        }

        var header = createMachineResponse.Headers.GetValues("Location")
            .ToList();
        if (header.Count != 1)
        {
            await this.OutputWriter.WriteError(
                "Expected exactly 1 value for 'Location' header, but {0} found",
                header.Count);
            return new(TestRunResult.Result.Failure);
        }

        var location = header[0];
        if (!location.Contains(payload.Code))
        {
            await this.OutputWriter.WriteError(
               "Expected location to contain '{0}', but it didn't. Location header contained '{1}'",
               payload.Code,
               location);
            return new(TestRunResult.Result.Failure);
        }

        return new(TestRunResult.Result.Success);
    }
}
