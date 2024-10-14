namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitionsVersions;

using MicroDotNet.Services.StateMachines.Application.AggregatesManager;
using MicroDotNet.Services.StateMachines.Domain.MachineDetails;
using MicroDotNet.Services.StateMachines.Domain.MachineStructure;

public static class Endpoints
{
    public const string CreateMachineVersionEndpointName = "V1_MachineDefinitions_Versions_Create";

    public const string GetMachineVersionEndpointName = "V1_MachineDefinitions_Versions_Get";

    public const string AcceptMachineVersionEndpointName = "V1_MachineDefinitions_Versions_Accept";

    public static async Task<IResult> CreateAsync(
        string code,
        LinkGenerator linkGenerator,
        IMachineVersionsService machineVersionsService,
        CancellationToken cancellationToken)
    {
        using var _ = machineVersionsService.Metrics.MeasureVersionCreationDuration();
        using var __ = machineVersionsService.Activities.StartVersionCreation(code);
        var output = await machineVersionsService.CreateAsync(
            code,
            cancellationToken)
            .ConfigureAwait(false);
        return output.Match(
            o =>
            {
                var link = linkGenerator.GetPathByName(
                    GetMachineVersionEndpointName,
                    values: new { code, version = o.Version });
                return Results.Created(link, o);
            },
            _ => Results.NotFound());
    }

    public static async Task<IResult> GetAsync(
        string code,
        short version,
        IMachineVersionsService machineVersionsService,
        CancellationToken cancellationToken)
    {
        using var _ = machineVersionsService.Metrics.MeasureVersionRetrievalDuration();
        using var __ = machineVersionsService.Activities.StartVersionRetrieval(code, version);
        var output = await machineVersionsService.GetAsync(
            new(code, version),
            cancellationToken)
            .ConfigureAwait(false);
        return output.Match(
            o => Results.Ok(o),
            _ => Results.NotFound());
    }

    public static async Task<IResult> AcceptAsync(
        string code,
        short version,
        IMachineVersionsService machineVersionsService,
        CancellationToken cancellationToken)
    {
        using var _ = machineVersionsService.Metrics.MeasureVersionAcceptationDuration();
        using var __ = machineVersionsService.Activities.StartVersionAcceptation(code, version);
        var output = await machineVersionsService.AcceptAsync(
            new(code, version),
            cancellationToken)
            .ConfigureAwait(false);
        return output.Match(
            o => Results.Ok(o),
            _ => Results.NotFound(),
            _ => Results.UnprocessableEntity());
    }

    public static void MapEndpoints(WebApplication app)
    {
        app.MapPost("/v1/machineDefinitions/{code}/versions", CreateAsync)
            .WithName(CreateMachineVersionEndpointName)
            .WithOpenApi();
        app.MapGet("/v1/machineDefinitions/{code}/versions/{version}", GetAsync)
            .WithName(GetMachineVersionEndpointName)
            .WithOpenApi();
        app.MapPost("/v1/machineDefinitions/{code}/versions/{version}/accept", AcceptAsync)
            .WithName(AcceptMachineVersionEndpointName)
            .WithOpenApi();
    }
}
