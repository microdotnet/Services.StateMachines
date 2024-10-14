namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitionsVersions;

using System.Diagnostics;

public sealed class Activities
{
    public const string ActivityName = "StateMachines.Api.MachineDefinitionsVersions";

    private const string VersionCreation = nameof(VersionCreation);

    private const string VersionRetrieval = nameof(VersionRetrieval);

    private const string VersionAcceptation = nameof(VersionAcceptation);

    private readonly ActivitySource activitySource;

    public Activities()
    {
        this.activitySource = new ActivitySource(ActivityName);
    }

    public Activity? StartVersionCreation(string code)
    {
        var result = this.activitySource.StartActivity(VersionCreation);
        if (result is null)
        {
            return null;
        }

        result.SetTag("MachineCode", code);
        return result;
    }

    public Activity? StartVersionRetrieval(string code, short version)
    {
        var result = this.activitySource.StartActivity(VersionRetrieval);
        if (result is null)
        {
            return null;
        }

        result.SetTag("MachineCode", code);
        result.SetTag("MachineVersion", version);
        return result;
    }

    public Activity? StartVersionAcceptation(string code, short version)
    {
        var result = this.activitySource.StartActivity(VersionAcceptation);
        if(result is null)
        {
            return null;
        }

        result.SetTag("MachineCode", code);
        result.SetTag("MachineVersion", version);
        return result;
    }
}
