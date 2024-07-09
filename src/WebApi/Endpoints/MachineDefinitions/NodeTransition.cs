namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.MachineDefinitions;

public class NodeTransition(
    string sourceNode,
    string targetNode)
{
    public string SourceNode { get; } = sourceNode;

    public string TargetNode { get; } = targetNode;
}
