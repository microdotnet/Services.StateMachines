namespace MicroDotNet.Services.StateMachines.Infrastructure.ReadModel.Mongo;

using MicroDotNet.Services.StateMachines.Application.ReadModel;

using Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMongoReadModel(
        this IServiceCollection services)
    {
        services.AddSingleton<IDatabaseProvider, DefaultDatabaseProvider>();
        services.AddSingleton<IClientProvider, DefaultClientProvider>();
        services.AddSingleton<ICollectionProvider, DefaultCollectionProvider>();

        services.AddTransient<IMachineDetailsRepository, MongoMachineDetailsRespository>();

        return services;
    }
}
