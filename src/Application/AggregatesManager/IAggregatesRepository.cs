namespace MicroDotNet.Services.StateMachines.Application.AggregatesManager
{
    using System.Threading.Tasks;
    using System.Threading;
    using System;

    using MicroDotNet.Services.StateMachines.Domain;

    public interface IAggregatesRepository<TAggregate>
        where TAggregate : class, IAggregate
    {
        Task<TAggregate?> FindAsync(string publicIdentifier, CancellationToken cancellationToken);

        Task<ulong> AddAsync(TAggregate aggregate, CancellationToken cancellationToken);

        Task<ulong> UpdateAsync(TAggregate aggregate, ulong? expectedRevision, CancellationToken cancellationToken);

        Task<ulong> DeleteAsync(TAggregate aggregate, ulong? expectedRevision, CancellationToken cancellationToken);
    }
}
