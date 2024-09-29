namespace MicroDotNet.Services.StateMachines.Domain.MachineStructure.Events
{
    public sealed class MachineConfirmed : MachineDefinitionEventBase
    {
        public MachineConfirmed(MachineName machineName)
            : base(machineName)
        {
        }
    }
}
