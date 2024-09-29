namespace MicroDotNet.Services.StateMachines.Application.ReadModel.MachineDetails
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public sealed class Machine
    {
        public Machine(
            Guid id,
            string code,
            string name,
            string description,
            IEnumerable<short> versions)
        {
            this.Id = id;
            this.Code = code;
            this.Name = name;
            this.Description = description;
            this.Versions = new ReadOnlyCollection<short>(versions.ToList());
        }

        public Guid Id { get; }

        public string Code { get; }

        public string Name { get; }

        public string Description { get; }

        public ReadOnlyCollection<short> Versions { get; }
    }
}
