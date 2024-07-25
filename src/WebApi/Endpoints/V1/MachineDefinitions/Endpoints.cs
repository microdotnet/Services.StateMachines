namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitions;

public static class Endpoints
{
    public const string CreateMachineEndpointName = "V1_CreateMachineDefinition";

    public const string GetMachineEndpointName = "V1_GetMachineDefinition";

    private const string UpdateMachineEndpointName = "V1_UpdateMachineDefinition";

    public static IResult Get(
        string code,
        short version,
        LinkGenerator linkGenerator)
    {
        var machine = FindMachine(code, version);
        if (machine is null)
        {
            return Results.NotFound();
        }

        var result = new MachineDefinition(
            machine.Id,
            machine.MachineCode,
            machine.MachineVersion,
            machine.Nodes,
            machine.Transitions);
        return Results.Ok(result);
    }

    public static IResult Post(
        LinkGenerator linkGenerator,
        PostPayload payload)
    {
        var machine = FindMachine(payload.Code, payload.Version);
        if (machine is not null)
        {
            return Results.BadRequest();
        }

        var id = Guid.NewGuid();
        var definitionToStore = new Db.MachineData(
            id,
            payload.Code,
            payload.Version,
            payload.Nodes,
            payload.Transitions);
        Db.AddMachineDefinition(definitionToStore);
        var link = linkGenerator.GetPathByName(
            GetMachineEndpointName,
            values: new { code = payload.Code, version = payload.Version });

        var result = new MachineDefinition(
            id,
            definitionToStore.MachineCode,
            definitionToStore.MachineVersion,
            definitionToStore.Nodes,
            definitionToStore.Transitions);
        return Results.Created(link, result);
    }

    public static IResult Put(
        LinkGenerator linkGenerator,
        string code,
        short version,
        PutPayload payload)
    {
        var machine = FindMachine(code, version);
        if (machine is null)
        {
            return Results.NotFound();
        }

        var id = machine.Id;
        var definitionToStore = new Db.MachineData(
            id,
            code,
            version,
            payload.Nodes,
            payload.Transitions);
        Db.UpdateMachineDefinition(definitionToStore);
        var link = linkGenerator.GetPathByName(
            GetMachineEndpointName,
            values: new { code, version });
        var result = new MachineDefinition(
           definitionToStore.Id,
           definitionToStore.MachineCode,
           definitionToStore.MachineVersion,
           definitionToStore.Nodes,
           definitionToStore.Transitions);
        return Results.Accepted(link, result);
    }

    public static void MapEndpoints(WebApplication app)
    {
        app.MapPost("/v1/machineDefinitions", Post)
            .WithName(CreateMachineEndpointName)
            .WithOpenApi();
        app.MapGet("/v1/machineDefinitions/{code}/{version}", Get)
            .WithName(GetMachineEndpointName)
            .WithOpenApi();
        app.MapPut("/v1/machineDefinitions/{code}/{version}", Put)
            .WithName(UpdateMachineEndpointName)
            .WithOpenApi();
    }

    private static Db.MachineData? FindMachine(string code, short version)
    {
        return Db.GetMachineDefinition(code, version);
    }
}
