namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitionsVersionsNodes;

using System.Diagnostics;

public sealed class Activities
{
    public const string ActivityName = "StateMachines.Api.MachineDefinitionsVersionsNodes";

    private const string AddNodes = nameof(AddNodes);

    private readonly ActivitySource activitySource;

    public Activities()
    {
        this.activitySource = new ActivitySource(ActivityName);
    }

    public Activity? StartAddingNodes(string code, short version, int nodesCount)
    {
        var result = this.activitySource.StartActivity(AddNodes);
        if (result is null)
        {
            return null;
        }

        result.SetTag("MachineCode", code);
        result.SetTag("MachineVersion", version);
        result.SetTag("NodesCount", nodesCount);
        return result;
    }
}
