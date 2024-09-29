namespace MicroDotNet.Services.StateMachines.Application.EventsMaterialization.EventHandlers.MachineStructure
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using MicroDotNet.Services.StateMachines.Application.ReadModel;
    using MicroDotNet.Services.StateMachines.Application.ReadModel.MachineDefinitions;
    using MicroDotNet.Services.StateMachines.Domain.MachineStructure.Events;

    using Microsoft.Extensions.Logging;

    public sealed class MachineDefinitionCreatedHandler : EventMaterializationHandlerBase<MachineDefinitionCreated>
    {
        private readonly IMachineDefinitionsRepository machineDefinitionsRepository;

        private readonly ILogger<MachineDefinitionCreatedHandler> logger;

        public MachineDefinitionCreatedHandler(
            IMachineDefinitionsRepository machineDefinitionsRepository,
            ILogger<MachineDefinitionCreatedHandler> logger)
        {
            this.machineDefinitionsRepository = machineDefinitionsRepository ?? throw new System.ArgumentNullException(nameof(machineDefinitionsRepository));
            this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        protected override async Task Handle(MachineDefinitionCreated @event, CancellationToken cancellationToken)
        {
            var machineDefinition = new MachineDefinition(
                @event.Id,
                @event.MachineName.Code,
                @event.MachineName.Version,
                Domain.MachineStructure.Status.InDesign.Code,
                Enumerable.Empty<Node>(),
                Enumerable.Empty<Transition>());
            var request = new CreateMachineDefinitionRequest(machineDefinition);
            this.logger.MachineDefinitionCreated(@event.MachineName.Code, @event.MachineName.Version);
            await this.machineDefinitionsRepository.CreateMachineDefinitionAsync(
                request,
                cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
