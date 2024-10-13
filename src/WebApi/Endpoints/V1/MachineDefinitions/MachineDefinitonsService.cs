namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitions;

using MicroDotNet.Services.StateMachines.Application.AggregatesManager;
using MicroDotNet.Services.StateMachines.Domain.MachineDetails;

public sealed class MachineDefinitonsService : IMachineDefinitionsService
{
    private readonly IAggregatesRepository<MachineDetailsAggregateRoot> aggregatesRepository;

    public MachineDefinitonsService(
        IAggregatesRepository<MachineDetailsAggregateRoot> aggregatesRepository)
    {
        this.aggregatesRepository = aggregatesRepository;
    }

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
}
