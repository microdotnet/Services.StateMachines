namespace MicroDotNet.Services.StateMachines.Application.EventsMaterialization.EventHandlers.MachineDetails
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using MicroDotNet.Services.StateMachines.Application.ReadModel;
    using MicroDotNet.Services.StateMachines.Application.ReadModel.MachineDetails;
    using MicroDotNet.Services.StateMachines.Domain.MachineDetails.Events;

    using Microsoft.Extensions.Logging;

    public sealed class VersionAddedHandler : EventMaterializationHandlerBase<VersionAdded>
    {
        private readonly IMachineDetailsRepository machineDetailsRepository;

        private readonly ILogger<VersionAddedHandler> logger;

        public VersionAddedHandler(
            IMachineDetailsRepository machineDetailsRepository,
            ILogger<VersionAddedHandler> logger)
        {
            this.machineDetailsRepository = machineDetailsRepository ?? throw new System.ArgumentNullException(nameof(machineDetailsRepository));
            this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        protected override async Task Handle(VersionAdded @event, CancellationToken cancellationToken)
        {
            var getMachineRequest = new GetMachineRequest(@event.Code);
            var getMachineResponse = await this.machineDetailsRepository.GetMachineAsync(getMachineRequest, cancellationToken)
                .ConfigureAwait(false);
            if (!getMachineResponse.IsFound)
            {
                throw new InvalidOperationException();
            }

            var sourceMachine = getMachineResponse.MachineDetails;
            var updatedMachine = new Machine(
                sourceMachine.Id,
                sourceMachine.Code,
                sourceMachine.Name,
                sourceMachine.Description,
                sourceMachine.Versions.Concat(new[] { @event.VersionNumber }));

            var updateMachineRequest = new UpdateMachineRequest(updatedMachine);
            this.logger.MaterializingVersionAdded(sourceMachine.Code, @event.VersionNumber);
            await this.machineDetailsRepository.UpdateMachineAsync(
                updateMachineRequest,
                cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
