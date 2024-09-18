namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitionsVersionsTransitions;

using MicroDotNet.Services.StateMachines.Application.AggregatesManager;
using MicroDotNet.Services.StateMachines.Domain.MachineStructure;
using System.Collections.ObjectModel;

public static class Endpoints
{
    private const string AddTransitionsEndpointName = "V1_MachineDefinitions_Versions_Transitions_Add";
    public static async Task<IResult> AddTransitions(
        string code,
        short version,
        AddTransitionsInput input,
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

        var transitions = new Collection<Transition>();
        foreach (var item in input.Transitions)
        {
            var source = (Node)item.SourceNode;
            var target = (Node)item.TargetNode;
            machine.AddTransition(source, target, item.TriggerName);
        }

        await machineDefinitionsRepository.UpdateAsync(
            machine,
            machine.Version,
            CancellationToken.None);
        return Results.Ok();
    }

    public static void MapEndpoints(WebApplication app)
    {
        app.MapPost("/v1/machineDefinitions/{code}/versions/{version}/transitions", AddTransitions)
            .WithName(AddTransitionsEndpointName)
            .WithOpenApi();
    }
}
