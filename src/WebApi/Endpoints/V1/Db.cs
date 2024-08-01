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

    public static void ConfirmMachine(Guid machineId)
    {
        var machine = machines[machineId];
        var confirmed = new MachineData(
            machine.Id,
            machine.MachineCode,
            machine.MachineVersion,
            machine.Nodes,
            machine.Transitions,
            true);
        UpdateMachineDefinition(confirmed);
    }

    public sealed class MachineData
    {
        public MachineData(
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
            this.Confirmed = false;
        }

        public MachineData(
            Guid id,
            string machineCode,
            short machineVersion,
            MachineNode[] nodes,
            NodeTransition[] transitions,
            bool confirmed)
        {
            this.Id = id;
            this.MachineCode = machineCode;
            this.MachineVersion = machineVersion;
            this.Nodes = nodes;
            this.Transitions = transitions;
            this.Confirmed = confirmed;
        }

        public Guid Id { get; }

        public string MachineCode { get; }

        public short MachineVersion { get; }

        public MachineNode[] Nodes { get; }

        public NodeTransition[] Transitions { get; }

        public bool Confirmed { get; }
    }
}
