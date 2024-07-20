namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints;

using MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1;

public static class WebApplicationExtensions
{
    public static WebApplication MapEndpoints(
        this WebApplication app)
    {
        app.MapV1Endpoints();
        return app;
    }
}