namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitionsVersionsNodes;

using System.Collections.ObjectModel;

using MicroDotNet.Services.StateMachines.Application.AggregatesManager;
using MicroDotNet.Services.StateMachines.Domain.MachineStructure;

public static class Endpoints
{
    private const string AddNodesEndpointName = "V1_MachineDefinitions_Versions_Nodes_Add";

    public static async Task<IResult> AddNodesAsync(
        string code,
        short version,
        AddNodesInput input,
        INodesService nodesService,
        CancellationToken cancellationToken)
    {
        var _ = nodesService.Metrics.MeasureNodesAddingDuration();
        var __ = nodesService.Activities.StartAddingNodes(code, version, input?.Nodes?.Count ?? 0);
        var output = await nodesService.AddNodesAsync(
            code,
            version,
            input,
            cancellationToken)
            .ConfigureAwait(false);

        return output.Match(
            o => Results.Ok(o),
            _ => Results.NotFound(),
            _ => Results.UnprocessableEntity());
    }

    public static void MapEndpoints(WebApplication app)
    {
        app.MapPost("/v1/machineDefinitions/{code}/versions/{version}/nodes", AddNodesAsync)
            .WithName(AddNodesEndpointName)
            .WithOpenApi();
    }
}
