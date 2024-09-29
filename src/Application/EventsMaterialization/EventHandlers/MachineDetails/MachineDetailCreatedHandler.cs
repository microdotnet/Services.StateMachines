namespace MicroDotNet.Services.StateMachines.Application.EventsMaterialization.EventHandlers.MachineDetails
{
    using System.Threading;
    using System.Threading.Tasks;

    using MicroDotNet.Services.StateMachines.Domain.MachineDetails.Events;

    using Microsoft.Extensions.Logging;

    public sealed class MachineDetailCreatedHandler : EventMaterializationHandlerBase<MachineDetailCreated>
    {
        private readonly ILogger<MachineDetailCreatedHandler> logger;

        public MachineDetailCreatedHandler(ILogger<MachineDetailCreatedHandler> logger)
        {
            this.logger = logger;
        }

        protected override async Task Handle(MachineDetailCreated @event, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
