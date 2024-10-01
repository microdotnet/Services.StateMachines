namespace MicroDotNet.Services.StateMachines.WebApi.HostedServices
{
    using System.Threading;
    using System.Threading.Tasks;

    using MicroDotNet.Services.StateMachines.Application;
    using MicroDotNet.Services.StateMachines.Application.EventsMaterialization;
    using MicroDotNet.Services.StateMachines.Application.EventsMaterialization.EventHandlers;

    public class ReadDatabaseProcessingService : BackgroundService
    {
        private readonly IEventsMaterialization eventsMaterializationProvider;

        private readonly IServiceProvider serviceProvider;

        private readonly ILogger<ReadDatabaseProcessingService> logger;

        public ReadDatabaseProcessingService(
            IEventsMaterialization eventsMaterializationProvider,
            IServiceProvider serviceProvider,
            ILogger<ReadDatabaseProcessingService> logger)
        {
            this.eventsMaterializationProvider = eventsMaterializationProvider ?? throw new ArgumentNullException(nameof(eventsMaterializationProvider));
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var request = new CreateSubscriptionRequest(
                "ReadDatabaseProcessing",
                this.HandleEvent);
            this.logger.LogError("STARTING SUBSCRIPTION FOR READ MODEL PROCESSING");
            return this.eventsMaterializationProvider.CreateSubscriptionAsync(
                request,
                stoppingToken);
        }

        private async Task HandleEvent(IEventEnvelope e, CancellationToken ct)
        {
            var handler = this.serviceProvider.GetKeyedService<IEventMaterializationHandler>(e.Data.GetType().FullName);
            if (handler != null)
            {
                await handler.Handle(e.Data, ct);
            }
            else
            {
                this.logger.MaterializationEventHandlerNotFound(e.Data.GetType());
                var message = string.Format(
                    ReadDatabaseProcessingServiceResources.MaterializationEventHandlerNotFound,
                    e.Data.GetType());
                throw new InvalidOperationException(message);
            }
        }
    }
}
