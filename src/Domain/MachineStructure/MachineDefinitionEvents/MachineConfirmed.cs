namespace MicroDotNet.Services.StateMachines.Domain.MachineStructure.MachineDefinitionEvents
{
    public sealed class MachineConfirmed : MachineDefinitionEventBase
    {
        public MachineConfirmed(MachineName machineName)
            : base(machineName)
        {
        }
    }
}
