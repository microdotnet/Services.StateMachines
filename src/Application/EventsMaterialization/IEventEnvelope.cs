namespace MicroDotNet.Services.StateMachines.Application.EventsMaterialization
{
    public interface IEventEnvelope
    {
        object Data { get; }

        EventMetadata Metadata { get; }
    }
}
