namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1;

using System.Collections.ObjectModel;

internal static class Db
{
    private static readonly Collection<Machine> machines = [];

    public static void AddMachineDefinition(Machine machine)
    {
        if (machines.Any(m => m.Code == machine.Code))
        {
            throw new InvalidOperationException();
        }

        machines.Add(machine);
    }

    public static Machine? GetMachineDefinition(string code)
    {
        return machines.FirstOrDefault(
            m => m.Code == code);
    }

    public static void UpdateMachineDefinition(Machine machine)
    {
        var existing = machines.First(x => x.Id == machine.Id);
        var index = machines.IndexOf(existing);
        machines[index] = machine;
    }

    public static void ConfirmMachine(Guid machineId)
    {
        var machine = machines.First(m => m.Id == machineId);
        machine.Confirmed = true;
    }

    internal sealed class Machine
    {
        public Guid Id { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public bool Confirmed { get; set; }

        public Collection<MachineVersion> Versions { get; } = [];
    }

    internal sealed class MachineVersion
    {
        public short Number { get; set; }
    }
}
