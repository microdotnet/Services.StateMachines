namespace MicroDotNet.Services.StateMachines.WebApi.HostedServices
{
    public static partial class ReadDatabaseProcessingServiceLogMessages
    {
        [LoggerMessage(
            LogLevel.Error,
            EventId = 1,
            EventName = nameof(MaterializationEventHandlerNotFound),
            Message = "Could not find materialization event handler for event type '{EventType}'.")]
        public static partial void MaterializationEventHandlerNotFound(
            this ILogger<ReadDatabaseProcessingService> logger,
            Type eventType);
    }
}
