namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitionsVersions;

using Microsoft.AspNetCore.Http.HttpResults;

public static class Endpoints
{
    public const string CreateMachineVersionEndpointName = "V1_MachineDefinitionsVersions_Create";

    public const string GetMachineVersionEndpointName = "V1_MachineDefinitionsVersions_Get";

    public static IResult Create(
        LinkGenerator linkGenerator,
        string code)
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

        var newVersion = new Db.MachineVersion { Number = (short)(lastVersion + 1) };
        machine.Versions.Add(newVersion);
        var output = new CreateOutput(newVersion.Number);
        var link = linkGenerator.GetPathByName(
            GetMachineVersionEndpointName,
            values: new { code, version = output.Version });
        return Results.Created(link, output);
    }

    public static IResult Get(
        string code,
        short version)
    {
        return Results.Ok();
    }

    public static void MapEndpoints(WebApplication app)
    {
        app.MapPost("/v1/machineDefinitions/{code}/versions", Create)
            .WithName(CreateMachineVersionEndpointName)
            .WithOpenApi();
        app.MapGet("/v1/machineDefinitions/{code}/versions/{version}", Get)
            .WithName(GetMachineVersionEndpointName)
            .WithOpenApi();
    }
}
