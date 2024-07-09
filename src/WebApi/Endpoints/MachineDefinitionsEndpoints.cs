namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints;

using System.Runtime.CompilerServices;
using MicroDotNet.Services.StateMachines.WebApi.Endpoints.MachineDefinitions;

public static class MachineDefinitionsEndpoints
{
    private static Dictionary<Guid, StateMachineDefinition> machines = [];
    
    public static IResult Get(string code, short version)
    {
        var machine = machines.Values.FirstOrDefault(d => d.MachineCode == code && d.MachineVersion == version);
        if (machine is null)
        {
            return Results.NotFound();
        }
    
        return Results.Ok(machine);
    }

    public static IResult Post(LinkGenerator linkGenerator, StateMachineDefinition definition)
    {
        var id = Guid.NewGuid();
        var definitionToStore = new StateMachineDefinition(id, definition.MachineCode, definition.MachineVersion);
        machines.Add(id, definitionToStore);
        var link = linkGenerator.GetPathByName(
            "GetMachineDefinition",
            values: new { code = definition.MachineCode, version = definition.MachineVersion });
        return Results.Created(link, id);
    }

    public static WebApplication MapMachineDefinitionsEndpoints(this WebApplication app)
    {
        app.MapGet("/machineDefinitions/{code}/{version}", Get)
            .WithName("GetMachineDefinition")
            .WithOpenApi();
        app.MapPost("/machineDefinitions", Post)
            .WithName("CreateMachineDefinition")
            .WithOpenApi();
        return app;
    }
}