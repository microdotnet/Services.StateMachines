namespace MicroDotNet.Services.StateMachines.Domain
{
    using System.Collections.Generic;
    using System;

    public interface IAggregate : IProjection
    {
        Guid Id { get; }

        ulong Version { get; }

        IEnumerable<object> DequeueUncommittedEvents();
    }
}
