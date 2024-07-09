namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.MachineDefinitions;

public class PutPayload(
    MachineNode[] nodes,
    NodeTransition[] transitions)
{
    public MachineNode[] Nodes { get; } = nodes;

    public NodeTransition[] Transitions { get; } = transitions;
}