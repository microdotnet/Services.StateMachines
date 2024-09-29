namespace MicroDotNet.Services.StateMachines.Application.EventsMaterialization
{
    public sealed class CreateSubscriptionResponse
    {
        public CreateSubscriptionResponse(string subscriptionId)
        {
            this.SubscriptionId = subscriptionId;
        }

        public string SubscriptionId { get; }
    }
}
