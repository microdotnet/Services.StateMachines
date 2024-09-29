namespace MicroDotNet.Services.StateMachines.Application.EventsMaterialization.EventHandlers
{
    using MicroDotNet.Services.StateMachines.Domain.MachineDetails.Events;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMaterializationEventHandlers(
            this IServiceCollection services)
        {
            services.AddKeyedTransient<IEventMaterializationHandler, MachineDetails.MachineDetailCreatedHandler>(typeof(MachineDetailCreated).FullName);

            return services;
        }
    }
}
