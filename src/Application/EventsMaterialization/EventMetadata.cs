namespace MicroDotNet.Services.StateMachines.Application.EventsMaterialization
{
    public sealed class EventMetadata
    {
        public EventMetadata(
            string eventId,
            ulong streamPosition,
            ulong logPosition)
        {
            this.EventId = eventId;
            this.StreamPosition = streamPosition;
            this.LogPosition = logPosition;
        }

        public string EventId { get; }

        public ulong StreamPosition { get; }

        public ulong LogPosition { get; }
    }
}