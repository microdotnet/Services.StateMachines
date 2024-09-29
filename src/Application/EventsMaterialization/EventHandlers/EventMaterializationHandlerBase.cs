namespace MicroDotNet.Services.StateMachines.Application.EventsMaterialization.EventHandlers
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;

    public abstract class EventMaterializationHandlerBase<TEvent> : IEventMaterializationHandler
    {
        public async Task Handle(object @event, CancellationToken cancellationToken)
        {
            if (@event is null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            if (!(@event is TEvent cast))
            {
                var message = string.Format(
                    CultureInfo.InvariantCulture,
                    EventMaterializationHandlerBaseResources.InvalidEventTypeProvided,
                    typeof(TEvent),
                    @event.GetType());
                throw new InvalidOperationException(message);
            }

            await this.Handle(cast, cancellationToken)
                .ConfigureAwait(false);
        }

        protected abstract Task Handle(TEvent @event, CancellationToken cancellationToken);
    }
}
