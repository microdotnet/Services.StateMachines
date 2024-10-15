namespace MicroDotNet.Services.StateMachines.WebApi;

using Autofac.Extensions.DependencyInjection;

using EventStore.Client.Extensions.OpenTelemetry;

using MicroDotNet.Services.StateMachines.Application.EventsMaterialization.EventHandlers;
using MicroDotNet.Services.StateMachines.Infrastructure.AggregatesManagers.EventStoreDb;
using MicroDotNet.Services.StateMachines.WebApi.Endpoints;
using MicroDotNet.Services.StateMachines.WebApi.OtelHelpers;

using Microsoft.Extensions.Hosting;

using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

public static class ApplicationBuilderExtensions
{
    private const string ServiceName = "Services.StateMachines";

    public static IHostBuilder SetupLogging(
        this IHostBuilder hostBuilder)
    {
        ////hostBuilder
        ////    .UseSerilog((ctx, cfg) =>
        ////    {
        ////        cfg.ConfigureLogger(ctx.Configuration);
        ////    });
        return hostBuilder;
    }

    public static void BuildConfiguration(
        this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureAppConfiguration(
            (context, configurationBuilder) =>
            {
                configurationBuilder.AddJsonFile("appsettings.json", false);
                if (context.HostingEnvironment.IsDevelopment())
                {
                    configurationBuilder.AddJsonFile("appsettings.Local.json", false);
                }

                configurationBuilder.AddEnvironmentVariables();
            });
    }

    public static IHostBuilder SetupContainer(
        this IHostBuilder hostBuilder)
    {
        hostBuilder.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        ////    new ContainerServiceProviderFactory<AutofacContainerBuilder, AutofacResolutionScope>());
        ////hostBuilder.ConfigureContainer<IModule>(module => module.AddModule(new WebApiModule(configuration)));
        return hostBuilder;
    }

    public static void AddOpenTelemetry(this WebApplicationBuilder builder)
    {
        builder.Logging.AddOpenTelemetry(options =>
        {
            options
                .SetResourceBuilder(ConfigureResourceBuilder(ResourceBuilder.CreateDefault()))
                .AddOtlpExporter();
        });

        builder.Services.AddOpenTelemetry()
            .ConfigureResource(resourceBuilder => ConfigureResourceBuilder(resourceBuilder))
            .WithTracing(tracing =>
                tracing
                    .AddAspNetCoreInstrumentation()
                    .AddEventStoreClientInstrumentation()
                    .AddEndpointsTraces()
                    .AddEventStoreDbTraces()
                    .AddOtlpExporter())
            .WithMetrics(metrics =>
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddEndpointsV1Meters()
                    .AddEventStoreDbMeters()
                    .AddOtlpExporter());
    }

    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddHostedService<HostedServices.ReadDatabaseProcessingService>();
        services.AddMaterializationEventHandlers();
        services.AddMetrics();
        services.StoreMachinesInEventStoreDb();
        services.AddEndpointsRegistrations();
        services.AddOtelHelpers();
    }

    public static void ConfigureApplication(this WebApplication app)
    {
        ////if (app.Environment.IsDevelopment())
        ////{
        app.UseSwagger();
        app.UseSwaggerUI();
        ////}

        app.MapSwagger();
        ////app.UseOpenTelemetryPrometheusScrapingEndpoint();
        app.MapEndpoints();
    }

    private static ResourceBuilder ConfigureResourceBuilder(ResourceBuilder resourceBuilder)
    {
        resourceBuilder
            .AddService(ServiceName)
            .AddEnvironmentVariableDetector()
            .AddDetector(sp => sp.GetService<EnvironmentDetector>()!);
        return resourceBuilder;
    }
}
