namespace MicroDotNet.Services.StateMachines.WebApi;

using MicroDotNet.Services.StateMachines.Infrastructure.AggregatesManagers.EventStoreDb;
using MicroDotNet.Services.StateMachines.WebApi.Endpoints;

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
        this IHostBuilder hostBuilder,
        IConfiguration configuration)
    {
        hostBuilder.ConfigureServices(sc => sc.StoreMachinesInEventStoreDb());
        ////hostBuilder.UseServiceProviderFactory(
        ////    new ContainerServiceProviderFactory<AutofacContainerBuilder, AutofacResolutionScope>());
        ////hostBuilder.ConfigureContainer<IModule>(module => module.AddModule(new WebApiModule(configuration)));
        return hostBuilder;
    }

    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        ////services.AddGrpc();
        ////services.Configure<QueueRepositorySettings>(configuration.GetSection("QueueRepository"));
        ////services.Configure<DocumentGeneratorSettings>(configuration.GetSection("DocumentGenerator"));
        ////services.Configure<RabbitMq.RawRabbitMqSettings>(configuration.GetSection("RabbitMq"));
        ////services.Configure<InternalServicesEndpoints>(configuration.GetSection("InternalServices"));
        ////services.AddHostedService<HostedServices.EventsProcessor>();
        ////services.AddHostedService<HostedServices.DocumentsGenerator>();

        ////services.AddGrpcClient<UsersManagement.UsersManagementClient>((serviceProvider, options) =>
        ////{
        ////    var addresses = serviceProvider.GetRequiredService<IOptions<InternalServicesEndpoints>>().Value;
        ////    options.Address = new(addresses.Users);
        ////});
    }

    public static void ConfigureApplication(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapSwagger();
        app.MapEndpoints();
    }

    ////private static void ConfigureLogger(this LoggerConfiguration cfg, IConfiguration configuration)
    ////{
    ////    cfg
    ////        .ReadFrom.Configuration(configuration);
    ////}
}
