namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitionsVersionsNodes;

public static class Endpoints
{
    private const string AddNodesEndpointName = "V1_MachineDefinitions_Versions_Nodes_Add";

    public static IResult AddNodes(
        string code,
        short version,
        AddNodesInput input)
    {
        return Results.Ok();
    }

    public static void MapEndpoints(WebApplication app)
    {
        app.MapPost("/v1/machineDefinitions/{code}/versions/{version}/nodes", AddNodes)
            .WithName(AddNodesEndpointName)
            .WithOpenApi();
    }
}
