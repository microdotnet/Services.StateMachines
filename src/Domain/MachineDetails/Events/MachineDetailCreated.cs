namespace MicroDotNet.Services.StateMachines.Domain.MachineDetails.Events
{
    using System;

    public sealed class MachineDetailCreated : MachineDetailsEventBase
    {
        public MachineDetailCreated(
            Guid id,
            string code,
            string name,
            string description)
            : base(code)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
        }

        public Guid Id { get; }

        public string Name { get; }

        public string Description { get; }
    }
}
