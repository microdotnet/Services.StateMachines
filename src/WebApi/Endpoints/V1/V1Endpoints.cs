namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1;

public static class V1Endpoints1
{
    public static WebApplication MapV1Endpoints(this WebApplication app)
    {
        MachineDefinitions.Endpoints.MapEndpoints(app);
        return app;
    }
}
