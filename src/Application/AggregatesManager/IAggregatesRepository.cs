namespace MicroDotNet.Services.StateMachines.Application.AggregatesManager
{
    using System.Threading.Tasks;
    using System.Threading;
    using System;

    using MicroDotNet.Services.StateMachines.Domain;

    public interface IAggregatesRepository<TAggregate>
        where TAggregate : class, IAggregate
    {
        Task<TAggregate?> Find(Guid id, CancellationToken cancellationToken);

        Task<ulong> Add(TAggregate aggregate, CancellationToken cancellationToken);

        Task<ulong> Update(TAggregate aggregate, ulong? expectedRevision, CancellationToken cancellationToken);

        Task<ulong> Delete(TAggregate aggregate, ulong? expectedRevision, CancellationToken cancellationToken);
    }
}
