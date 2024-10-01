namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitions;

using MicroDotNet.Services.StateMachines.Application.AggregatesManager;
using MicroDotNet.Services.StateMachines.Application.ReadModel;
using MicroDotNet.Services.StateMachines.Application.ReadModel.MachineDetails;
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
        CreateInput payload,
        CancellationToken cancellationToken)
    {
        var id = Guid.NewGuid();
        var machineAggregate = MachineDetailsAggregateRoot.Create(
            id,
            payload.Code,
            payload.Name,
            payload.Description);
        await aggregatesRepository.AddAsync(machineAggregate, cancellationToken)
            .ConfigureAwait(false);
        var link = linkGenerator.GetPathByName(
            GetMachineEndpointName,
            values: new { code = payload.Code });
        var output = new CreateOutput(id);
        return Results.Created(link, output);
    }

    public static async Task<IResult> Get(
        IMachineDetailsRepository machineDetailsRepository,
        string code,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return Results.NotFound();
        }

        var machineDetails = await machineDetailsRepository.GetMachineAsync(
            new GetMachineRequest(code),
            cancellationToken)
            .ConfigureAwait(false);
        return machineDetails.Match(
            r =>
            {
                var output = new GetOutput(
                    r.MachineDetails.Code,
                    r.MachineDetails.Name,
                    r.MachineDetails.Description);
                return Results.Ok(output);
            },
            _ => Results.NotFound());
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
