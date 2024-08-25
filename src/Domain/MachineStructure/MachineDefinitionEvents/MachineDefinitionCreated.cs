namespace MicroDotNet.Services.StateMachines.Domain.MachineStructure.MachineDefinitionEvents
{
    public sealed class MachineDefinitionCreated
    {
        private MachineDefinitionCreated(
            MachineName machineName)
        {
            this.MachineName = machineName;
        }

        public MachineName MachineName { get; }

        public static MachineDefinitionCreated Create(
            MachineName machineName)
        {
            return new MachineDefinitionCreated(machineName);
        }
    }
}
