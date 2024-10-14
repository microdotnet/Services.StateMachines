namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitions;

using MicroDotNet.Services.StateMachines.Application.AggregatesManager;
using MicroDotNet.Services.StateMachines.Domain.MachineDetails;

using OneOf;
using OneOf.Types;

public sealed class MachineDefinitonsService : IMachineDefinitionsService
{
    private readonly IAggregatesRepository<MachineDetailsAggregateRoot> aggregatesRepository;

    public MachineDefinitonsService(
        Activities activities,
        Metrics metrics,
        IAggregatesRepository<MachineDetailsAggregateRoot> aggregatesRepository)
    {
        this.Activities = activities ?? throw new ArgumentNullException(nameof(activities));
        this.Metrics = metrics ?? throw new ArgumentNullException(nameof(metrics));
        this.aggregatesRepository = aggregatesRepository;
    }

    public Activities Activities { get; }

    public Metrics Metrics { get; }

    public async Task<CreateOutput> CreateAsync(
        CreateInput input,
        CancellationToken cancellationToken)
    {
        var id = Guid.NewGuid();
        var machineAggregate = MachineDetailsAggregateRoot.Create(
            id,
            input.Code,
            input.Name,
            input.Description);
        await aggregatesRepository.AddAsync(machineAggregate, CancellationToken.None)
            .ConfigureAwait(false);
        return new CreateOutput(id);
    }

    public async Task<OneOf<GetOutput, NotFound>> GetAsync(
        string code,
        CancellationToken cancellationToken)
    {
        var publicIdentifier = MachineDetailsAggregateRoot.CreatePublicIdentifier(code);
        var aggregate = await this.aggregatesRepository.FindAsync(publicIdentifier, cancellationToken)
            .ConfigureAwait(false);
        if (aggregate is null)
        {
            return new NotFound();
        }

        return new GetOutput(
            aggregate.Code,
            aggregate.Name,
            aggregate.Description);
    }
}
