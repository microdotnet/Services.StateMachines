namespace MicroDotNet.Services.StateMachines.Infrastructure.AggregatesManagers.EventStoreDb
{
    using EventStore.Client;

    using Grpc.Core;

    using Microsoft.Extensions.Logging;

    public static partial class SubscriptionRunnerLogMessages
    {
        [LoggerMessage(
            LogLevel.Warning,
            EventId = 1,
            EventName = nameof(EventDeserializationFailed),
            Message = "Couldn't deserialize event with id: {EventId}")]
        public static partial void EventDeserializationFailed(
            this ILogger<SubscriptionRunner> logger,
            Uuid eventId);

        [LoggerMessage(
            LogLevel.Error,
            EventId = 2,
            EventName = nameof(MessageConsumptionError),
            Message = "[{SubscriptionName}] - Error consuming message {ExceptionMessage}{ExceptionStackTrace}")]
        public static partial void MessageConsumptionError(
            this ILogger<SubscriptionRunner> logger,
            string subscriptionName,
            string exceptionMessage,
            string? exceptionStackTrace,
            Exception ex);

        [LoggerMessage(
            LogLevel.Warning,
            EventId = 3,
            EventName = nameof(SubscriptionDroppedByClient),
            Message = "Subscription '{SubscriptionId}' dropped by client")]
        public static partial void SubscriptionDroppedByClient(
            this ILogger<SubscriptionRunner> logger,
            string subscriptionId);

        [LoggerMessage(
            LogLevel.Error,
            EventId = 4,
            EventName = nameof(SubscriptionDroppedWithError),
            Message = "Subscription to all '{SubscriptionName}' dropped with '{StatusCode}' and '{Reason}'.")]
        public static partial void SubscriptionDroppedWithError(
            this ILogger<SubscriptionRunner> logger,
            string subscriptionName,
            StatusCode statusCode,
            SubscriptionDroppedReason reason,
            Exception? ex);

        [LoggerMessage(
            LogLevel.Error,
            EventId = 5,
            EventName = nameof(ResubscriptionFailed),
            Message = "Failed to resubscribe to all '{SubscriptionId}'. Error information: '{ExceptionMessage}{ExceptionStackTrace}'")]
        public static partial void ResubscriptionFailed(
            this ILogger<SubscriptionRunner> logger,
            string subscriptionId,
            string exceptionMessage,
            string? exceptionStackTrace,
            Exception ex);

        [LoggerMessage(
            LogLevel.Warning,
            EventId = 6,
            EventName = nameof(NoDataEvent),
            Message = "Event without data ('{EventId}') received")]
        public static partial void NoDataEvent(
            this ILogger<SubscriptionRunner> logger,
            Uuid eventId);

        [LoggerMessage(
            LogLevel.Debug,
            EventId = 7,
            EventName = nameof(CheckpointEventFound),
            Message = "Checkpoint event - ignoring")]
        public static partial void CheckpointEventFound(
            this ILogger<SubscriptionRunner> logger);
    }
}
