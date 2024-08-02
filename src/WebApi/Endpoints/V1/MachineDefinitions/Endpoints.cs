namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitions;

public static class Endpoints
{
    public const string CreateMachineEndpointName = "V1_MachineDefinitions_Create";

    public const string GetMachineEndpointName = "V1_MachineDefinitions_Get";

    public static IResult Create(
        LinkGenerator linkGenerator,
        CreateInput payload)
    {
        var machine = new Db.Machine
        {
            Id = Guid.NewGuid(),
            Code = payload.Code,
            Name = payload.Name,
            Description = payload.Description,
            Confirmed = false,
        };
        Db.AddMachineDefinition(machine);

        var link = linkGenerator.GetPathByName(
            GetMachineEndpointName,
            values: new { code = payload.Code });
        var output = new CreateOutput(machine.Id);
        return Results.Created(link, output);
    }

    public static IResult Get(
        string code)
    {
        return Results.Ok();
    }

    public static void MapEndpoints(WebApplication app)
    {
        app.MapPost("/v1/machineDefinitions", Create)
            .WithName(CreateMachineEndpointName)
            .WithOpenApi();
        app.MapGet("/v1/machineDefinitions/{code}", Get)
            .WithName(GetMachineEndpointName)
            .WithOpenApi();
    }
}
