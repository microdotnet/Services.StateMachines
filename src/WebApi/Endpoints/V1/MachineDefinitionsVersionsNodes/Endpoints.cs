namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitionsVersionsNodes;

public static class Endpoints
{
    private const string AddNodesEndpointName = "V1_MachineDefinitions_Versions_Nodes_Add";

    public static IResult AddNodes(
        string code,
        short version,
        AddNodesInput input)
    {
        var machine = Db.GetMachineDefinition(code);
        if (machine is null)
        {
            return Results.NotFound();
        }

        var lastVersion = 0;
        if (machine.Versions.Count != 0)
        {
            lastVersion = machine.Versions.Max(v => v.Number);
        }

        if (lastVersion == 0)
        {
            return Results.NotFound();
        }

        if (machine.Confirmed)
        {
            return Results.UnprocessableEntity();
        }

        return Results.Ok();
    }

    public static void MapEndpoints(WebApplication app)
    {
        app.MapPost("/v1/machineDefinitions/{code}/versions/{version}/nodes", AddNodes)
            .WithName(AddNodesEndpointName)
            .WithOpenApi();
    }
}
