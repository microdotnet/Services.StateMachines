namespace MicroDotNet.Services.StateMachines.Application.EventsMaterialization.EventHandlers
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IEventMaterializationHandler
    {
        Task Handle(object @event, CancellationToken cancellationToken);
    }
}
