namespace MicroDotNet.Services.StateMachines.Application.ReadModel.MachineDefinitions
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public sealed class MachineDefinition
    {
        public MachineDefinition(
            Guid id,
            string code,
            short version,
            string status,
            IEnumerable<Node> nodes,
            IEnumerable<Transition> transitions)
        {
            this.Id = id;
            this.Code = code;
            this.Version = version;
            this.Status = status;
            this.Nodes = new ReadOnlyCollection<Node>(nodes.ToList());
            this.Transitions = new ReadOnlyCollection<Transition>(transitions.ToList());
        }

        public Guid Id { get; }

        public string Code { get; }

        public short Version { get; }

        public string Status { get; }

        public ReadOnlyCollection<Node> Nodes { get; }

        public ReadOnlyCollection<Transition> Transitions { get; }
    }
}
