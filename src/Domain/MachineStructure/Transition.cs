namespace MicroDotNet.Services.StateMachines.Domain.MachineStructure
{
    using System;
    using System.Diagnostics;

    [DebuggerDisplay("Transition from '{From}' to '{To}'")]
    public sealed class Transition
    {
        private Transition(Node from, Node to)
        {
            this.From = from;
            this.To = to;
        }

        public Node From { get; }

        public Node To { get; }

        public static Transition Create(Node from, Node to)
        {
            if (from is null)
            {
                throw new ArgumentNullException(
                    nameof(from),
                    TransitionResources.Create_FromNodeNull);
            }

            if (to is null)
            {
                throw new ArgumentNullException(
                    nameof(to),
                    TransitionResources.Create_ToNodeNull);
            }

            return new Transition(from, to);
        }
    }
}