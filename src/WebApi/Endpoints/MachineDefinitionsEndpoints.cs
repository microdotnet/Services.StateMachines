namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints;

using MicroDotNet.Services.StateMachines.WebApi.Endpoints.MachineDefinitions;

public static class MachineDefinitionsEndpoints
{
    private static readonly Dictionary<Guid, MachineData> machines = [];

    public static IResult Get(
        string code,
        short version)
    {
        var machine = machines.Values.FirstOrDefault(d => d.MachineCode == code && d.MachineVersion == version);
        if (machine is null)
        {
            return Results.NotFound();
        }

        var result = new GetResult(
            machine.Id,
            machine.MachineCode,
            machine.MachineVersion,
            machine.Nodes,
            machine.Transitions);
        return Results.Ok(result);
    }

    public static IResult Post(
        LinkGenerator linkGenerator,
        string code,
        short version,
        PostPayload payload)
    {
        var machine = FindMachine(code, version);
        if (machine is not null)
        {
            return Results.BadRequest();
        }

        var id = Guid.NewGuid();
        var definitionToStore = new MachineData(
            id,
            code,
            version,
            payload.Nodes,
            payload.Transitions);
        machines.Add(id, definitionToStore);
        var link = linkGenerator.GetPathByName(
            "GetMachineDefinition",
            values: new { code, version });
        return Results.Created(link, id);
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
        var definitionToStore = new MachineData(
            id,
            code,
            version,
            payload.Nodes,
            payload.Transitions);
        machines[id] = definitionToStore;
        var link = linkGenerator.GetPathByName(
            "GetMachineDefinition",
            values: new { code, version });
        return Results.Accepted(link, id);
    }

    public static WebApplication MapMachineDefinitionsEndpoints(this WebApplication app)
    {
        app.MapGet("/machineDefinitions/{code}/{version}", Get)
            .WithName("GetMachineDefinition")
            .WithOpenApi();
        app.MapPost("/machineDefinitions", Post)
            .WithName("CreateMachineDefinition")
            .WithOpenApi();
        app.MapPut("/machineDefinitions/{code}/{version}", Put)
            .WithName("UpdateMachineDefinition")
            .WithOpenApi();
        return app;
    }

    private static MachineData? FindMachine(string code, short version)
    {
        return machines.Values.FirstOrDefault(m => m.MachineCode == code && m.MachineVersion == version);
    }

    private class MachineData(
        Guid id,
        string machineCode,
        short machineVersion,
        MachineNode[] nodes,
        NodeTransition[] transitions)
    {
        public Guid Id { get; } = id;

        public string MachineCode { get; } = machineCode;

        public short MachineVersion { get; } = machineVersion;

        public MachineNode[] Nodes { get; } = nodes;

        public NodeTransition[] Transitions { get; } = transitions;
    }
}