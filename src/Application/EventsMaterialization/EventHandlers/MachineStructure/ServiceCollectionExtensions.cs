namespace MicroDotNet.Services.StateMachines.Application.EventsMaterialization.EventHandlers.MachineStructure
{
    using MicroDotNet.Services.StateMachines.Domain.MachineStructure.Events;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMachineDefinitionsMaterialization(
            this IServiceCollection services)
        {
            services.RegisterEventMaterializationHandler<MachineDefinitionCreatedHandler, MachineDefinitionCreated>();
            services.RegisterEventMaterializationHandler<NodesAddedHandler, NodesAdded>();
            services.RegisterEventMaterializationHandler<TransitionAddedHandler, TransitionAdded>();
            services.RegisterEventMaterializationHandler<MachineConfirmedHandler, MachineConfirmed>();

            return services;
        }
    }
}
