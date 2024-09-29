namespace MicroDotNet.Services.StateMachines.Application.EventsMaterialization.EventHandlers.MachineDetails
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using MicroDotNet.Services.StateMachines.Application.ReadModel;
    using MicroDotNet.Services.StateMachines.Application.ReadModel.MachineDetails;
    using MicroDotNet.Services.StateMachines.Domain.MachineDetails.Events;

    using Microsoft.Extensions.Logging;

    public sealed class MachineDetailCreatedHandler : EventMaterializationHandlerBase<MachineDetailCreated>
    {
        private readonly IMachineDetailsRepository machineDetailsRepository;

        private readonly ILogger<MachineDetailCreatedHandler> logger;

        public MachineDetailCreatedHandler(
            IMachineDetailsRepository machineDetailsRepository,
            ILogger<MachineDetailCreatedHandler> logger)
        {
            this.machineDetailsRepository = machineDetailsRepository ?? throw new System.ArgumentNullException(nameof(machineDetailsRepository));
            this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        protected override async Task Handle(MachineDetailCreated @event, CancellationToken cancellationToken)
        {
            var machine = new Machine(
                @event.Id,
                @event.Code,
                @event.Name,
                @event.Description,
                Enumerable.Empty<short>());
            var request = new CreateMachineRequest(machine);
            this.logger.MaterializingMachine(machine.Code);
            await this.machineDetailsRepository.CreateMachineAsync(request, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
