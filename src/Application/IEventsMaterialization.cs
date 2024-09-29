namespace MicroDotNet.Services.StateMachines.Application
{
    using System.Threading;
    using System.Threading.Tasks;

    using MicroDotNet.Services.StateMachines.Application.EventsMaterialization;

    public interface IEventsMaterialization
    {
        Task<CreateSubscriptionResponse> CreateSubscriptionAsync(
            CreateSubscriptionRequest request,
            CancellationToken stopProcessingToken);
    }
}
