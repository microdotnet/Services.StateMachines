namespace MicroDotNet.Services.StateMachines.WebApi.HostedServices
{
    using System.Threading;
    using System.Threading.Tasks;

    using MicroDotNet.Services.StateMachines.Application;
    using MicroDotNet.Services.StateMachines.Application.EventsMaterialization;

    public class ReadDatabaseProcessingService : BackgroundService
    {
        private readonly IEventsMaterialization eventsMaterializationProvider;

        public ReadDatabaseProcessingService(IEventsMaterialization eventsMaterializationProvider)
        {
            this.eventsMaterializationProvider = eventsMaterializationProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(
                async () =>
                {
                    var request = new CreateSubscriptionRequest(
                        "ReadDatabaseProcessing",
                        this.HandleEvent);
                    await this.eventsMaterializationProvider.CreateSubscriptionAsync(
                        request,
                        stoppingToken);
                });
        }

        private async Task HandleEvent(IEventEnvelope e, CancellationToken ct)
        {
            await Task.CompletedTask;
            Console.WriteLine($"Processing event '{e.Metadata.EventId}'");
            ////this.logger.LogDebug(
            ////    "Handling event type '{EventType}'",
            ////    e.Data.GetType().FullName);
            ////await this.materializationEngine.MaterializeAsync(e, ct)
            ////    .ConfigureAwait(false);
        }
    }
}
