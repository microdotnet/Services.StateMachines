namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitionsVersions;

using System.Threading;
using System.Threading.Tasks;

using MicroDotNet.Services.StateMachines.Application.AggregatesManager;
using MicroDotNet.Services.StateMachines.Domain.MachineDetails;
using MicroDotNet.Services.StateMachines.Domain.MachineStructure;

using OneOf;
using OneOf.Types;

public class MachineVersionsService : IMachineVersionsService
{
    private readonly IAggregatesRepository<MachineDetailsAggregateRoot> machinesDetailsReposiory;

    private readonly IAggregatesRepository<MachineDefinitionAggregateRoot> machineDefinitionsRepository;

    public MachineVersionsService(
        Activities activities,
        Metrics metrics,
        IAggregatesRepository<MachineDetailsAggregateRoot> machinesDetailsReposiory,
        IAggregatesRepository<MachineDefinitionAggregateRoot> machineDefinitionsRepository)
    {
        this.Activities = activities ?? throw new ArgumentNullException(nameof(activities));
        this.Metrics = metrics ?? throw new ArgumentNullException(nameof(metrics));
        this.machinesDetailsReposiory = machinesDetailsReposiory ?? throw new ArgumentNullException(nameof(machinesDetailsReposiory));
        this.machineDefinitionsRepository = machineDefinitionsRepository ?? throw new ArgumentNullException(nameof(machineDefinitionsRepository));
    }

    public Activities Activities { get; }

    public Metrics Metrics { get; }

    public async Task<OneOf<CreateOutput, NotFound>> CreateAsync(string code, CancellationToken cancellationToken)
    {
        var machine = await machinesDetailsReposiory.FindAsync(
            MachineDetailsAggregateRoot.CreatePublicIdentifier(code),
            CancellationToken.None);
        if (machine is null)
        {
            return new NotFound();
        }


        short newVersion = machine.AddNewVersion();
        var machineDefinition = MachineDefinitionAggregateRoot.Create(Domain.MachineName.Create(machine.Code, newVersion));
        await machineDefinitionsRepository.AddAsync(machineDefinition, CancellationToken.None);
        await machinesDetailsReposiory.UpdateAsync(machine, machine.Version, CancellationToken.None);
        return new CreateOutput(machineDefinition.MachineName.Version);
    }

    public async Task<OneOf<GetOutput, NotFound>> GetAsync(GetInput input, CancellationToken cancellationToken)
    {
        var publicIdentifier = MachineDefinitionAggregateRoot.CreatePublicIdentifier(input.Code, input.Version);
        var machine = await machineDefinitionsRepository.FindAsync(
            publicIdentifier,
            CancellationToken.None);

        if (machine is null)
        {
            return new NotFound();
        }

        var result = new GetOutput(
            machine.MachineName.Code,
            machine.MachineName.Version,
            machine.Status.Code,
            machine.Nodes.Select(n => new GetOutput.Node(n.Name)).ToList(),
            machine.Transitions.Select(t => new GetOutput.Transition((string)t.Source, (string)t.Target, t.Trigger)).ToList());
        return result;
    }

    public async Task<OneOf<AcceptOutput, NotFound, Error>> AcceptAsync(AcceptInput input, CancellationToken cancellationToken)
    {
        var publicIdentifier = MachineDefinitionAggregateRoot.CreatePublicIdentifier(input.Code, input.Version);
        var machine = await this.machineDefinitionsRepository.FindAsync(
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

        machine.Confirm();
        await machineDefinitionsRepository.UpdateAsync(
            machine,
            machine.Version,
            CancellationToken.None);
        var currentMachine = await this.machineDefinitionsRepository.FindAsync(
            publicIdentifier,
            CancellationToken.None)
            .ConfigureAwait(false);
        return new AcceptOutput(
            currentMachine!.MachineName.Code,
            currentMachine.MachineName.Version,
            currentMachine.Status.Code,
            currentMachine.Nodes.Select(n => new AcceptOutput.Node(n.Name)).ToList(),
            currentMachine.Transitions.Select(t => new AcceptOutput.Transition((string)t.Source, (string)t.Target, t.Trigger)).ToList());
    }
}
