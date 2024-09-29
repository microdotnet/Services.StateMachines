namespace MicroDotNet.Services.StateMachines.Domain.MachineStructure.MachineDefinitionEvents
{
    using System;

    public sealed class MachineDefinitionCreated : MachineDefinitionEventBase
    {
        public MachineDefinitionCreated(
            Guid id,
            MachineName machineName)
            : base(machineName)
        {
            this.Id = id;
        }

        public Guid Id { get; }
    }
}
