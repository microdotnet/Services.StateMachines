namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitions;

public class GetResult
{
    public GetResult(
        Guid id,
        string machineCode,
        short machineVersion,
        MachineNode[] nodes,
        NodeTransition[] transitions)
    {
        this.Id = id;
        this.MachineCode = machineCode;
        this.MachineVersion = machineVersion;
        this.Nodes = nodes;
        this.Transitions = transitions;
    }

    public Guid Id { get; }

    public string MachineCode { get; }

    public short MachineVersion { get; }

    public MachineNode[] Nodes { get; }

    public NodeTransition[] Transitions { get; }
}
