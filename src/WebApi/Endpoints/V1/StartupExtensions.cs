namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1;

using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

public static class StartupExtensions
{
    public static IServiceCollection AddV1EndpointsRegistrations(
        this IServiceCollection services)
    {
        services.AddScoped<MachineDefinitions.IMachineDefinitionsService, MachineDefinitions.MachineDefinitonsService>();
        services.AddSingleton<MachineDefinitions.MachineDefinitionsMetrics>();
        services.AddSingleton<MachineDefinitions.MachineDefinitionsActivities>();
        return services;
    }

    public static TracerProviderBuilder AddV1Traces(
        this TracerProviderBuilder builder)
    {
        builder
            .AddSource(MachineDefinitions.MachineDefinitionsActivities.ActivityName);
        return builder;
    }

    public static MeterProviderBuilder AddV1Meters(
        this MeterProviderBuilder builder)
    {
        builder
            .AddMeter(MachineDefinitions.MachineDefinitionsMetrics.MeterName);
        return builder;
    }
}
