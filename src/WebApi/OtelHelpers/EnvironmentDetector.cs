namespace MicroDotNet.Services.StateMachines.WebApi.OtelHelpers;

using OpenTelemetry.Resources;

public class EnvironmentDetector : IResourceDetector
{
    public Resource Detect()
    {
        return ResourceBuilder.CreateEmpty()
            .AddAttributes(new Dictionary<string, object>
            {
                {  "environment.name", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "NO_ENVIRONMENT_NAME" }
            }).Build();
    }
}
