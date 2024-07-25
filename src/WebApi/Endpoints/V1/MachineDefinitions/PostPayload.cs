namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitions;

public class PostPayload
{
    public PostPayload(
        string code,
        short version,
        MachineNode[] nodes,
        NodeTransition[] transitions)
    {
        this.Code = code;
        this.Version = version;
        this.Nodes = nodes;
        this.Transitions = transitions;
    }

    public string Code { get; }

    public short Version { get; }

    public MachineNode[] Nodes { get; }

    public NodeTransition[] Transitions { get; }
}