namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints;

using MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1;

using OpenTelemetry.Metrics;

using OpenTelemetry.Trace;

public static class StartupExtensions
{
    public static IServiceCollection AddEndpointsRegistrations(
        this IServiceCollection services)
    {
        services.AddV1EndpointsRegistrations();
        return services;
    }

    public static TracerProviderBuilder AddEndpointsTraces(
        this TracerProviderBuilder builder)
    {
        builder
            .AddV1Traces();
        return builder;
    }
    public static MeterProviderBuilder AddEndpointsV1Meters(
        this MeterProviderBuilder builder)
    {
        builder
            .AddV1Meters();
        return builder;
    }
}
