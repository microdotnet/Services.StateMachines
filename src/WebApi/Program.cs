using MicroDotNet.Services.StateMachines.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Host.SetupLogging();
builder.Host.BuildConfiguration();
builder.Host.SetupContainer();

builder.Services.ConfigureServices();

var app = builder.Build();

app.ConfigureApplication();

app.Run();

namespace MicroDotNet.Services.StateMachines.WebApi
{
    public sealed partial class WebApiProgram { }
}