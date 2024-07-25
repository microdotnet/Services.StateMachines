namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1;

using MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitions;

internal static class Db
{
    private static readonly Dictionary<Guid, MachineData> machines = [];

    public static void AddMachineDefinition(MachineData machine)
    {
        machines.Add(machine.Id, machine);
    }

    public static void UpdateMachineDefinition(MachineData machine)
    {
        machines[machine.Id] = machine;
    }

    public static MachineData? GetMachineDefinition(string code, short version)
    {
        return machines.Values.FirstOrDefault(d => d.MachineCode == code && d.MachineVersion == version);
    }

    public class MachineData(
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
}
