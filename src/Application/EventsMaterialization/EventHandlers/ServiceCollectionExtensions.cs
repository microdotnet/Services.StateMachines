namespace MicroDotNet.Services.StateMachines.Application.EventsMaterialization.EventHandlers
{
    using MicroDotNet.Services.StateMachines.Application.EventsMaterialization.EventHandlers.MachineDetails;
    using MicroDotNet.Services.StateMachines.Application.EventsMaterialization.EventHandlers.MachineStructure;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMaterializationEventHandlers(
            this IServiceCollection services)
        {
            services.AddMachineDetailsMaterialization();
            services.AddMachineDefinitionsMaterialization();

            return services;
        }

        internal static IServiceCollection RegisterEventMaterializationHandler<THandler, TEvent>(
            this IServiceCollection services)
            where THandler : class, IEventMaterializationHandler
        {
            services.AddKeyedTransient<IEventMaterializationHandler, THandler>(typeof(TEvent).FullName);
            return services;
        }
    }
}
