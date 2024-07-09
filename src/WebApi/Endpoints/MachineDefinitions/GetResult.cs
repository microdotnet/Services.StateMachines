namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.MachineDefinitions;

using System.Xml.Linq;

public class GetResult(
    Guid id,
    string machineCode,
    short machineVersion,
    MachineNode[] nodes,
    NodeTransition[] transitions)
{
    public Guid Id { get; } = id;

    public string MachineCode { get; } = machineCode;

    public short MachineVersion { get; } = machineVersion;

    public MachineNode[] Nodes { get; } = nodes;

    public NodeTransition[] Transitions { get; } = transitions;
}
