namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitionsVersionsTransitions;

public static class Endpoints
{
    private const string AddTransitionsEndpointName = "V1_MachineDefinitions_Versions_Transitions_Add";

    public static IResult AddTransitions(
        AddTransitionsInput input)
    {
        return Results.Ok();
    }

    public static void MapEndpoints(WebApplication app)
    {
        app.MapPost("/v1/machineDefinitions/{code}/versions/{version}/transitions", AddTransitions)
            .WithName(AddTransitionsEndpointName)
            .WithOpenApi();
    }
}
