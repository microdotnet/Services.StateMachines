using MicroDotNet.Services.StateMachines.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Host.SetupLogging();
builder.Host.BuildConfiguration();
builder.Host.SetupContainer();

builder.Services.ConfigureServices(builder.Configuration);

builder.AddOpenTelemetry();

var app = builder.Build();

app.ConfigureApplication();

app.Run();

public sealed partial class WebApiProgram { }