namespace MicroDotNet.Services.StateMachines.Application.EventsMaterialization.EventHandlers.MachineStructure
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using MicroDotNet.Services.StateMachines.Application.ReadModel;
    using MicroDotNet.Services.StateMachines.Application.ReadModel.MachineDefinitions;
    using MicroDotNet.Services.StateMachines.Domain.MachineStructure.Events;

    using Microsoft.Extensions.Logging;

    public sealed class TransitionAddedHandler : EventMaterializationHandlerBase<TransitionAdded>
    {
        private readonly IMachineDefinitionsRepository machineDefinitionsRepository;

        private readonly ILogger<TransitionAddedHandler> logger;

        public TransitionAddedHandler(
            IMachineDefinitionsRepository machineDefinitionsRepository,
            ILogger<TransitionAddedHandler> logger)
        {
            this.machineDefinitionsRepository = machineDefinitionsRepository ?? throw new System.ArgumentNullException(nameof(machineDefinitionsRepository));
            this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        protected override async Task Handle(TransitionAdded @event, CancellationToken cancellationToken)
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
                currentDefinition.Status,
                currentDefinition.Nodes,
                currentDefinition.Transitions.Concat(new[] { new Transition((string)@event.Source, (string)@event.Target, @event.Trigger) }));
            var updateMachineRequest = new UpdateMachineDefinitionRequest(
                newDefinition);
            await this.machineDefinitionsRepository.UpdateMachineDefinitionAsync(
                updateMachineRequest,
                cancellationToken)
                .ConfigureAwait(false);
            this.logger.TransitionAdded(@event.MachineName.Code, @event.MachineName.Version, @event.Source, @event.Target, @event.Trigger);
        }
    }
}
