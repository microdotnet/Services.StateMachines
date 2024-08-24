namespace MicroDotNet.Services.StateMachines.Domain.MachineStructure
{
    using System;
    using System.Diagnostics;

    [DebuggerDisplay("Transition from '{From}' to '{To}'")]
    public sealed class Transition
    {
        private Transition(Node source, Node target, string trigger)
        {
            this.Source = source;
            this.Target = target;
            this.Trigger = trigger;
        }

        public Node Source { get; }

        public Node Target { get; }

        public string Trigger { get; }

        public static Transition Create(Node source, Node target, string trigger)
        {
            if (source is null || string.IsNullOrWhiteSpace(source.Name))
            {
                throw new ArgumentNullException(
                    nameof(source),
                    TransitionResources.Create_SourceNodeNull);
            }

            if (target is null || string.IsNullOrWhiteSpace(target.Name))
            {
                throw new ArgumentNullException(
                    nameof(target),
                    TransitionResources.Create_TargetNodeNull);
            }

            if (string.IsNullOrWhiteSpace(trigger))
            {
                throw new ArgumentNullException(
                    nameof(trigger),
                    TransitionResources.Create_TriggerEmpty);
            }

            return new Transition(source, target, trigger);
        }
    }
}