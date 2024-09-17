namespace MicroDotNet.Services.StateMachines.Domain
{
    using System.Collections.Generic;
    using System;

    public interface IAggregate : IProjection
    {
        Guid Id { get; }

        string PublicIdentifier { get; }

        ulong Version { get; }

        IEnumerable<object> DequeueUncommittedEvents();
    }
}
