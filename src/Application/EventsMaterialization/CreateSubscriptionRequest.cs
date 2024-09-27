namespace MicroDotNet.Services.StateMachines.Application.EventsMaterialization
{
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class CreateSubscriptionRequest
    {
        public delegate Task StreamEventHandler(
            IEventEnvelope envelope,
            CancellationToken cancellationToken);

        public CreateSubscriptionRequest(
            string subscriptionName,
            StreamEventHandler handler)
        {
            this.SubscriptionName = subscriptionName;
            this.Handler = handler;
        }

        public string SubscriptionName { get; }

        public StreamEventHandler Handler { get; }
    }
}
