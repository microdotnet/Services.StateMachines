namespace MicroDotNet.Services.StateMachines.Domain.MachineStructure.MachineDefinitionEvents
{
    using System;

    public sealed class MachineDefinitionCreated
    {
        public MachineDefinitionCreated(
            Guid id,
            MachineName machineName)
        {
            this.Id = id;
            this.MachineName = machineName;
        }

        public Guid Id { get; }

        public MachineName MachineName { get; }
    }
}
