namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1;

using MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitions;
using MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitionsVersions;

using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

public static class StartupExtensions
{
    public static IServiceCollection AddV1EndpointsRegistrations(
        this IServiceCollection services)
    {
        services.AddMachineDefinitionsEndpointsRegistrations();
        services.AddMachineDefinitionsVersionsEndpointsRegistrations();
        return services;
    }

    public static TracerProviderBuilder AddV1Traces(
        this TracerProviderBuilder builder)
    {
        builder
            .AddMachineDefinitionsTraces()
            .AddMachineDefinitionsVersionsTraces();
        return builder;
    }

    public static MeterProviderBuilder AddV1Meters(
        this MeterProviderBuilder builder)
    {
        builder
            .AddMachineDefinitionsMeters()
            .AddMachineDefinitionsVersionsMeters();
        return builder;
    }
}
