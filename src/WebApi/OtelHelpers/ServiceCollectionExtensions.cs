namespace MicroDotNet.Services.StateMachines.WebApi.OtelHelpers;

using OpenTelemetry.Resources;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOtelHelpers(
        this IServiceCollection services)
    {
        services
            .AddSingleton<EnvironmentDetector>();
        return services;
    }
}
