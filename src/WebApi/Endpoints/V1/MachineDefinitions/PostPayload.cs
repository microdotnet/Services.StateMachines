namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitions;

public class PostPayload
{
    public PostPayload(
        MachineNode[] nodes,
        NodeTransition[] transitions)
    {
        this.Nodes = nodes;
        this.Transitions = transitions;
    }

    public MachineNode[] Nodes { get; }

    public NodeTransition[] Transitions { get; }
}