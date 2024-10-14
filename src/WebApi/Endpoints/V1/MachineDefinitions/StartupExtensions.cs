namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitions;

using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

public static class StartupExtensions
{
    public static IServiceCollection AddMachineDefinitionsEndpointsRegistrations(
        this IServiceCollection services)
    {
        services.AddScoped<IMachineDefinitionsService, MachineDefinitonsService>();
        services.AddSingleton<Metrics>();
        services.AddSingleton<Activities>();
        return services;
    }

    public static TracerProviderBuilder AddMachineDefinitionsTraces(
        this TracerProviderBuilder builder)
    {
        builder
            .AddSource(Activities.ActivityName);
        return builder;
    }

    public static MeterProviderBuilder AddMachineDefinitionsMeters(
        this MeterProviderBuilder builder)
    {
        builder
            .AddMeter(Metrics.MeterName);
        return builder;
    }
}
