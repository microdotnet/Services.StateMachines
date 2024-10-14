namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitionsVersionsNodes;

using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

using MicroDotNet.Services.StateMachines.Application.AggregatesManager;

using MicroDotNet.Services.StateMachines.Domain.MachineStructure;

using OneOf;
using OneOf.Types;

public class NodesService : INodesService
{
    private readonly IAggregatesRepository<MachineDefinitionAggregateRoot> machineDefinitionsRepository;

    public NodesService(
        Activities activities,
        Metrics metrics,
        IAggregatesRepository<MachineDefinitionAggregateRoot> machineDefinitionsRepository)
    {
        this.Activities = activities ?? throw new ArgumentNullException(nameof(activities));
        this.Metrics = metrics ?? throw new ArgumentNullException(nameof(metrics));
        this.machineDefinitionsRepository = machineDefinitionsRepository ?? throw new ArgumentNullException(nameof(machineDefinitionsRepository));
    }

    public Activities Activities { get; }

    public Metrics Metrics { get; }

    public async Task<OneOf<AddNodesOutput, NotFound, Error>> AddNodesAsync(string code, short version, AddNodesInput input, CancellationToken cancellationToken)
    {
        var publicIdentifier = MachineDefinitionAggregateRoot.CreatePublicIdentifier(code, version);
        var machine = await machineDefinitionsRepository.FindAsync(
            publicIdentifier,
            CancellationToken.None);

        if (machine is null)
        {
            return new NotFound();
        }

        if (machine.Status != Status.InDesign)
        {
            return new Error();
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

        return new();
    }
}
