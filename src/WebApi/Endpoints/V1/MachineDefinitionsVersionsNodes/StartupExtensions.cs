namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitionsVersionsNodes;

using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

public static class StartupExtensions
{
    public static IServiceCollection AddMachineDefinitionsVersionsNodesEndpointsRegistrations(
        this IServiceCollection services)
    {
        services.AddScoped<INodesService, NodesService>();
        services.AddSingleton<Metrics>();
        services.AddSingleton<Activities>();
        return services;
    }

    public static TracerProviderBuilder AddMachineDefinitionsVersionsNodesTraces(
        this TracerProviderBuilder builder)
    {
        builder
            .AddSource(Activities.ActivityName);
        return builder;
    }

    public static MeterProviderBuilder AddMachineDefinitionsVersionsNodesMeters(
        this MeterProviderBuilder builder)
    {
        builder
            .AddMeter(Metrics.MeterName);
        return builder;
    }
}
