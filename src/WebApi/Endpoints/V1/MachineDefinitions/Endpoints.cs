namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitions;

using System.Diagnostics.Metrics;

using MicroDotNet.Services.StateMachines.Application.AggregatesManager;
using MicroDotNet.Services.StateMachines.Domain;
using MicroDotNet.Services.StateMachines.Domain.MachineDetails;
using MicroDotNet.Services.StateMachines.Domain.MachineStructure;

public static class Endpoints
{
    public const string CreateMachineEndpointName = "V1_MachineDefinitions_Create";

    public const string GetMachineEndpointName = "V1_MachineDefinitions_Get";

    public static async Task<IResult> CreateAsync(
        LinkGenerator linkGenerator,
        IMachineDefinitionsService machineDefinitionsService,
        MachineDefinitionsMetrics metrics,
        MachineDefinitionsActivities activities,
        CreateInput payload,
        CancellationToken cancellationToken)
    {
        using var _ = metrics.MeasureMachineCreationDuration();
        using var __ = activities.StartMachineCreation();
        try
        {
            var output = await machineDefinitionsService.CreateAsync(
                payload,
                cancellationToken)
                .ConfigureAwait(false);
            var link = linkGenerator.GetPathByName(
                GetMachineEndpointName,
                values: new { code = payload.Code });
            return Results.Created(link, output);
        }
        finally
        {
            metrics.IncreaseMachineCreationCount();
        }
    }

    public static IResult Get(
        string code)
    {
        return Results.Problem();
    }

    public static void MapEndpoints(WebApplication app)
    {
        app.MapPost("/v1/machineDefinitions", CreateAsync)
            .WithName(CreateMachineEndpointName)
            .WithOpenApi();
        app.MapGet("/v1/machineDefinitions/{code}", Get)
            .WithName(GetMachineEndpointName)
            .WithOpenApi();
    }
}
