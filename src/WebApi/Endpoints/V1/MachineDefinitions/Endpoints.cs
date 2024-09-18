namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitions;

using MicroDotNet.Services.StateMachines.Application.AggregatesManager;
using MicroDotNet.Services.StateMachines.Domain;
using MicroDotNet.Services.StateMachines.Domain.MachineDetails;
using MicroDotNet.Services.StateMachines.Domain.MachineStructure;

public static class Endpoints
{
    public const string CreateMachineEndpointName = "V1_MachineDefinitions_Create";

    public const string GetMachineEndpointName = "V1_MachineDefinitions_Get";

    public static async Task<IResult> Create(
        LinkGenerator linkGenerator,
        IAggregatesRepository<MachineDetailsAggregateRoot> aggregatesRepository,
        CreateInput payload)
    {
        var id = Guid.NewGuid();
        var machineAggregate = MachineDetailsAggregateRoot.Create(
            id,
            payload.Code,
            payload.Name,
            payload.Description);
        await aggregatesRepository.AddAsync(machineAggregate, CancellationToken.None)
            .ConfigureAwait(false);
        var link = linkGenerator.GetPathByName(
            GetMachineEndpointName,
            values: new { code = payload.Code });
        var output = new CreateOutput(id);
        return Results.Created(link, output);
    }

    public static IResult Get(
        string code)
    {
        return Results.Problem();
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
