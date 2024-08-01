namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitions;

public sealed class NodeTransition
{
    public NodeTransition(
        string sourceNode,
        string targetNode)
    {
        this.SourceNode = sourceNode;
        this.TargetNode = targetNode;
    }

    public string SourceNode { get; }

    public string TargetNode { get; }
}
