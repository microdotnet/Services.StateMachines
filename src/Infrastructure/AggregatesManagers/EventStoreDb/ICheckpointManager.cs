namespace MicroDotNet.Services.StateMachines.Infrastructure.AggregatesManagers.EventStoreDb
{
    public interface ICheckpointManager
    {
        Task SetLastCheckpoint(string subscriptionId, ulong position, CancellationToken ct);

        Task<ulong?> GetLastCheckpoint(string subscriptionId, CancellationToken ct);
    }
}
