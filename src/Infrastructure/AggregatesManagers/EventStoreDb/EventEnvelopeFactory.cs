namespace MicroDotNet.Services.StateMachines.Infrastructure.AggregatesManagers.EventStoreDb
{
    using System;
    using MicroDotNet.Services.StateMachines.Application.EventsMaterialization;

    public static class EventEnvelopeFactory
    {
        public static EventEnvelope<T> From<T>(T data)
            where T : notnull
        {
            return new(data, new EventMetadata(Guid.NewGuid().ToString(), 0, 0));
        }

        public static IEventEnvelope From(object data, EventMetadata metadata)
        {
            // TODO: Get rid of reflection!
            var type = typeof(EventEnvelope<>).MakeGenericType(data.GetType());
            return (IEventEnvelope)Activator.CreateInstance(type, data, metadata)!;
        }
    }
}
