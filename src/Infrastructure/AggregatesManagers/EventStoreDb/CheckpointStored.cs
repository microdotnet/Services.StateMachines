namespace MicroDotNet.Services.StateMachines.Infrastructure.AggregatesManagers.EventStoreDb
{
    public record CheckpointStored(string SubscriptionId, ulong? Position, DateTime CheckpointedAt);
}
