namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitionsVersions;

using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

public static class StartupExtensions
{
    public static IServiceCollection AddMachineDefinitionsVersionsEndpointsRegistrations(
        this IServiceCollection services)
    {
        services.AddScoped<IMachineVersionsService, MachineVersionsService>();
        services.AddSingleton<Metrics>();
        services.AddSingleton<Activities>();
        return services;
    }

    public static TracerProviderBuilder AddMachineDefinitionsVersionsTraces(
        this TracerProviderBuilder builder)
    {
        builder
            .AddSource(Activities.ActivityName);
        return builder;
    }

    public static MeterProviderBuilder AddMachineDefinitionsVersionsMeters(
        this MeterProviderBuilder builder)
    {
        builder
            .AddMeter(Metrics.MeterName);
        return builder;
    }
}
