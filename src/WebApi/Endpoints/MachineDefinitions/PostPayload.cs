namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.MachineDefinitions;

public class PostPayload(
    MachineNode[] nodes,
    NodeTransition[] transitions)
{
    public MachineNode[] Nodes { get; } = nodes;

    public NodeTransition[] Transitions { get; } = transitions;
}