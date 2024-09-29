namespace MicroDotNet.Services.StateMachines.Domain.MachineStructure.MachineDefinitionEvents
{
    public abstract class MachineDefinitionEventBase
    {
        protected MachineDefinitionEventBase(
            MachineName machineName)
        {
            this.MachineName = machineName;
        }

        public MachineName MachineName { get; }
    }
}
