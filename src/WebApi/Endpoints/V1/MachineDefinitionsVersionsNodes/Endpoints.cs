namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitionsVersionsNodes;

using System.Collections.ObjectModel;

using MicroDotNet.Services.StateMachines.Application.AggregatesManager;
using MicroDotNet.Services.StateMachines.Domain.MachineStructure;

public static class Endpoints
{
    private const string AddNodesEndpointName = "V1_MachineDefinitions_Versions_Nodes_Add";

    public static async Task<IResult> AddNodes(
        string code,
        short version,
        AddNodesInput input,
        IAggregatesRepository<MachineDefinitionAggregateRoot> machineDefinitionsRepository)
    {
        try
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

            var nodes = new Collection<Node>();
            foreach (var item in input.Nodes)
            {
                nodes.Add(Node.Create(item.Name));
            }

            machine.AddNodes([.. nodes]);
            await machineDefinitionsRepository.UpdateAsync(
                machine,
                machine.Version,
                CancellationToken.None);
            return Results.Ok();
        }
        catch(Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
            throw;
        }
    }

    public static void MapEndpoints(WebApplication app)
    {
        app.MapPost("/v1/machineDefinitions/{code}/versions/{version}/nodes", AddNodes)
            .WithName(AddNodesEndpointName)
            .WithOpenApi();
    }
}
