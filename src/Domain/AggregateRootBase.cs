namespace MicroDotNet.Services.StateMachines.Domain
{
    using System;
    using System.Collections.Generic;

    public abstract class AggregateRootBase : IAggregate
    {
        private readonly Queue<object> uncommitedEvents = new Queue<object>();

        private ulong? version = null;

        protected AggregateRootBase()
        {
        }

        public Guid Id { get; protected set; }

        public abstract string PublicIdentifier { get; }

        public ulong Version => this.version ?? 0;

        public IEnumerable<object> DequeueUncommittedEvents()
        {
            var events = this.uncommitedEvents.ToArray();
            this.uncommitedEvents.Clear();
            return events;
        }

        public virtual void When(object @event)
        {
            this.IncreaseVersion();
        }

        protected void Enqueue(object @event)
        {
            this.uncommitedEvents.Enqueue(@event);
        }

        protected void IncreaseVersion()
        {
            if (this.version is null)
            {
                this.version = 0;
            }
            else
            {
                this.version++;
            }
        }
    }
}
