namespace MicroDotNet.Services.StateMachines.Domain.MachineStructure.Events
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
