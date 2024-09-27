namespace MicroDotNet.Services.StateMachines.Infrastructure.AggregatesManagers.EventStoreDb
{
    using MicroDotNet.Services.StateMachines.Application.EventsMaterialization;

    public sealed class EventEnvelope<T> : IEventEnvelope
        where T : notnull
    {
        public EventEnvelope(
            T data,
            EventMetadata metadata)
        {
            this.Data = data;
            this.Metadata = metadata;
        }

        public T Data { get; }

        object IEventEnvelope.Data => this.Data;

        public EventMetadata Metadata { get; }
    }
}
