namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints;

public static class WebApplicationExtensions
{
    public static WebApplication MapEndpoints(
        this WebApplication app)
    {
        app.MapMachineDefinitionsEndpoints();
        return app;
    }
}