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

    public sealed class NodesAddedHandler : EventMaterializationHandlerBase<NodesAdded>
    {
        private readonly IMachineDefinitionsRepository machineDefinitionsRepository;

        private readonly ILogger<NodesAddedHandler> logger;

        public NodesAddedHandler(
            IMachineDefinitionsRepository machineDefinitionsRepository,
            ILogger<NodesAddedHandler> logger)
        {
            this.machineDefinitionsRepository = machineDefinitionsRepository ?? throw new System.ArgumentNullException(nameof(machineDefinitionsRepository));
            this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        protected override async Task Handle(NodesAdded @event, CancellationToken cancellationToken)
        {
            var getMachineRequest = new GetMachineDefinitionRequest(@event.MachineName.Code, @event.MachineName.Version);
            var getMachineResponse = await this.machineDefinitionsRepository.GetMachineDefinitionAsync(
                getMachineRequest,
                cancellationToken)
                .ConfigureAwait(false);
            if (getMachineResponse.IsT1)
            {
                throw new InvalidOperationException();
            }

            var currentDefinition = getMachineResponse.AsT0.Machine;
            var newDefinition = new MachineDefinition(
                currentDefinition.Id,
                currentDefinition.Code,
                currentDefinition.Version,
                currentDefinition.Status,
                currentDefinition.Nodes.Concat(@event.Nodes.Select(n => new Node(n.Name))),
                currentDefinition.Transitions);
            var updateMachineRequest = new UpdateMachineDefinitionRequest(
                newDefinition);
            await this.machineDefinitionsRepository.UpdateMachineDefinitionAsync(
                updateMachineRequest,
                cancellationToken)
                .ConfigureAwait(false);
            this.logger.NodesAdded(@event.MachineName.Code, @event.MachineName.Version, @event.Nodes.Length);
        }
    }
}
