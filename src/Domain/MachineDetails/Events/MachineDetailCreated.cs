namespace MicroDotNet.Services.StateMachines.Domain.MachineDetails.Events
{
    using System;

    public sealed class MachineDetailCreated
    {
        public MachineDetailCreated(
            Guid id,
            string code,
            string name,
            string description)
        {
            this.Id = id;
            this.Code = code;
            this.Name = name;
            this.Description = description;
        }

        public Guid Id { get; }

        public string Code { get; }

        public string Name { get; }

        public string Description { get; }
    }
}
