namespace MicroDotNet.Services.StateMachines.Application.EventsMaterialization.EventHandlers.MachineStructure
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using MicroDotNet.Services.StateMachines.Application.ReadModel;
    using MicroDotNet.Services.StateMachines.Application.ReadModel.MachineDefinitions;
    using MicroDotNet.Services.StateMachines.Domain.MachineStructure.Events;

    using Microsoft.Extensions.Logging;

    public class MachineConfirmedHandler : EventMaterializationHandlerBase<MachineConfirmed>
    {
        private readonly IMachineDefinitionsRepository machineDefinitionsRepository;

        private readonly ILogger<MachineConfirmedHandler> logger;

        public MachineConfirmedHandler(
            IMachineDefinitionsRepository machineDefinitionsRepository,
            ILogger<MachineConfirmedHandler> logger)
        {
            this.machineDefinitionsRepository = machineDefinitionsRepository ?? throw new System.ArgumentNullException(nameof(machineDefinitionsRepository));
            this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        protected override async Task Handle(MachineConfirmed @event, CancellationToken cancellationToken)
        {
            var getMachineRequest = new GetMachineDefinitionRequest(@event.MachineName.Code, @event.MachineName.Version);
            var getMachineResponse = await this.machineDefinitionsRepository.GetMachineDefinitionAsync(
                getMachineRequest,
                cancellationToken)
                .ConfigureAwait(false);
            if (getMachineResponse == null)
            {
                throw new InvalidOperationException();
            }

            var currentDefinition = getMachineResponse.Machine;
            var newDefinition = new MachineDefinition(
                currentDefinition.Id,
                currentDefinition.Code,
                currentDefinition.Version,
                Domain.MachineStructure.Status.Completed.Code,
                currentDefinition.Nodes,
                currentDefinition.Transitions);
            var updateMachineRequest = new UpdateMachineDefinitionRequest(
                newDefinition);
            await this.machineDefinitionsRepository.UpdateMachineDefinitionAsync(
                updateMachineRequest,
                cancellationToken)
                .ConfigureAwait(false);
            this.logger.MachineConfirmed(@event.MachineName.Code, @event.MachineName.Version);
        }
    }
}
