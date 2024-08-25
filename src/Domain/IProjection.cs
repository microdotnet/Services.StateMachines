namespace MicroDotNet.Services.StateMachines.Domain
{
    public interface IProjection
    {
        void When(object @event);
    }
}
