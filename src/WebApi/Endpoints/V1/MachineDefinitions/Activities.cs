namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitions;

using System.Diagnostics;

public sealed class Activities
{
    public const string ActivityName = "StateMachines.Api.MachineDefinitions";

    private const string MachineCreation = nameof(MachineCreation);

    private const string MachineRetrieval = nameof(MachineRetrieval);

    private readonly ActivitySource activitySource;

    public Activities()
    {
        this.activitySource = new ActivitySource(ActivityName);
    }

    public Activity? StartMachineCreation(string code)
    {
        var result = this.activitySource.StartActivity(MachineCreation);
        if (result is null)
        {
            return null;
        }

        result.SetTag("MachineCode", code);
        return result;
    }

    public Activity? StartMachineRetrieval(string code)
    {
        var result = this.activitySource.StartActivity(MachineRetrieval);
        if (result is null)
        {
            return null;
        }

        result.SetTag("MachineCode", code);
        return result;
    }
}
