using MicroDotNet.Services.StateMachines.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Host.SetupLogging();
builder.Host.BuildConfiguration();
builder.Host.SetupContainer(builder.Configuration);

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

app.ConfigureApplication();

app.Run();

public sealed partial class WebApiProgram { }