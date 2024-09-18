namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitionsVersions;

using MicroDotNet.Services.StateMachines.Application.AggregatesManager;
using MicroDotNet.Services.StateMachines.Domain.MachineDetails;
using MicroDotNet.Services.StateMachines.Domain.MachineStructure;

using Microsoft.AspNetCore.Http.HttpResults;

public static class Endpoints
{
    public const string CreateMachineVersionEndpointName = "V1_MachineDefinitions_Versions_Create";

    public const string GetMachineVersionEndpointName = "V1_MachineDefinitions_Versions_Get";

    public const string AcceptMachineVersionEndpointName = "V1_MachineDefinitions_Versions_Accept";

    public static async Task<IResult> Create(
        LinkGenerator linkGenerator,
        IAggregatesRepository<MachineDetailsAggregateRoot> machinesDetailsReposiory,
        IAggregatesRepository<MachineDefinitionAggregateRoot> machineDefinitionsRepository,
        string code)
    {
        var machine = await machinesDetailsReposiory.Find(
            MachineDetailsAggregateRoot.CreatePublicIdentifier(code),
            CancellationToken.None);
        if (machine is null)
        {
            return Results.NotFound();
        }


        short newVersion = machine.AddNewVersion();
        var machineDefinition = MachineDefinitionAggregateRoot.Create(Domain.MachineName.Create(machine.Code, newVersion));
        await machineDefinitionsRepository.AddAsync(machineDefinition, CancellationToken.None);
        await machinesDetailsReposiory.UpdateAsync(machine, machine.Version, CancellationToken.None);
        var output = new CreateOutput(machineDefinition.MachineName.Version);
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

    public static async Task<IResult> Accept(
        string code,
        short version,
        IAggregatesRepository<MachineDefinitionAggregateRoot> machineDefinitionsRepository)
    {
        var publicIdentifier = MachineDefinitionAggregateRoot.CreatePublicIdentifier(code, version);
        var machine = await machineDefinitionsRepository.Find(
            publicIdentifier,
            CancellationToken.None);

        if (machine is null)
        {
            return Results.NotFound();
        }

        if (machine.Status != Status.InDesign)
        {
            return Results.UnprocessableEntity();
        }

        machine.Confirm();
        await machineDefinitionsRepository.UpdateAsync(
            machine,
            machine.Version,
            CancellationToken.None);
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
        app.MapPost("/v1/machineDefinitions/{code}/versions/{version}/accept", Accept)
            .WithName(AcceptMachineVersionEndpointName)
            .WithOpenApi();
    }
}
