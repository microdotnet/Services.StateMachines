namespace MicroDotNet.Services.StateMachines.Application.EventsMaterialization.EventHandlers.MachineDetails
{
    using MicroDotNet.Services.StateMachines.Domain.MachineDetails.Events;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMachineDetailsMaterialization(
            this IServiceCollection services)
        {
            services.RegisterEventMaterializationHandler<MachineDetailCreatedHandler, MachineDetailCreated>();
            services.RegisterEventMaterializationHandler<VersionAddedHandler, VersionAdded>();

            return services;
        }
    }
}
