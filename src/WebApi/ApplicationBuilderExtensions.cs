namespace MicroDotNet.Services.StateMachines.WebApi;

using Autofac.Extensions.DependencyInjection;

using MicroDotNet.Services.StateMachines.Application.EventsMaterialization.EventHandlers;
using MicroDotNet.Services.StateMachines.Infrastructure.AggregatesManagers.EventStoreDb;
using MicroDotNet.Services.StateMachines.WebApi.Endpoints;

using Microsoft.Extensions.Hosting;

using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

public static class ApplicationBuilderExtensions
{
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
        var resourceBuilder = ResourceBuilder.CreateDefault()
            .AddService(serviceName: "statemachines.api", serviceVersion: "1.0.0");

        builder.Services.AddOpenTelemetry()
            .WithTracing(tracing => tracing
                .SetResourceBuilder(resourceBuilder)
                .AddSource("HelloOpenTelemetry")
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddConsoleExporter()
            )
            .WithMetrics(metrics => metrics
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddConsoleExporter()
                .AddMeter(
                        "Microsoft.AspNetCore.Hosting",
                        "Microsoft.AspNetCore.Server.Kestrel",
                        "System.Net.Http",
                        Endpoints.V1.MachineDefinitions.MachineDefinitionsMetrics.MeterName)
            );

        builder.Logging.AddOpenTelemetry(logging => { logging.AddConsoleExporter(); });
    }

    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddHostedService<HostedServices.ReadDatabaseProcessingService>();
        services.AddMaterializationEventHandlers();
        services.AddSingleton<Endpoints.V1.MachineDefinitions.MachineDefinitionsMetrics>();
        services.AddMetrics();
        services.StoreMachinesInEventStoreDb();
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

    ////private static void ConfigureLogger(this LoggerConfiguration cfg, IConfiguration configuration)
    ////{
    ////    cfg
    ////        .ReadFrom.Configuration(configuration);
    ////}
}
