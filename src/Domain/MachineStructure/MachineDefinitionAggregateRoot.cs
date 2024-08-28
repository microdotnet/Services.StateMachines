namespace MicroDotNet.Services.StateMachines.Domain.MachineStructure
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;

    using MicroDotNet.Services.StateMachines.Domain.MachineStructure.MachineDefinitionEvents;

    public sealed class MachineDefinitionAggregateRoot : AggregateRootBase
    {
        private readonly Collection<Node> nodes = new Collection<Node>();

        private readonly Collection<Transition> transitions = new Collection<Transition>();

        private MachineDefinitionAggregateRoot(MachineName machineName)
        {
            this.MachineName = machineName;
            var @event = MachineDefinitionCreated.Create(machineName);
            this.Enqueue(@event);
            this.Apply(@event);
        }

        public MachineName MachineName { get; private set; }

        public static MachineDefinitionAggregateRoot Create(MachineName machineName)
        {
            if (machineName is null)
            {
                throw new ArgumentNullException(nameof(machineName));
            }

            return new MachineDefinitionAggregateRoot(machineName);
        }

        public void AddNodes(Node[] nodes)
        {
            if (nodes is null)
            {
                throw new ArgumentNullException(nameof(nodes));
            }

            if (nodes.Length == 0)
            {
                throw new ArgumentException(
                    MachineDefinitionAggregateRootResources.AddNodes_NodesToAddEmpty,
                    nameof(nodes));
            }

            var @event = MachineNodesAdded.Create(nodes);
            this.Enqueue(@event);
            this.Apply(@event);
        }

        public void AddTransition(
            Node source,
            Node target,
            string trigger)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (target is null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (string.IsNullOrEmpty(trigger))
            {
                throw new ArgumentException(MachineDefinitionAggregateRootResources.AddTransition_TriggerMustNotBeEmpty, nameof(trigger));
            }

            if (!this.nodes.Any(n => n.Name == source.Name))
            {
                throw new InvalidOperationException(
                    string.Format(
                        MachineDefinitionAggregateRootResources.AddTransition_SourceNodeDoesNotExist,
                        source.Name));
            }

            if (!this.nodes.Any(n => n.Name == target.Name))
            {
                throw new InvalidOperationException(
                    string.Format(
                        MachineDefinitionAggregateRootResources.AddTransition_TargetNodeDoesNotExist,
                        target.Name));
            }

            if (this.transitions.Any(t => t.Source.Name == source.Name && t.Target.Name == target.Name && t.Trigger == trigger))
            {
                throw new InvalidOperationException(
                    string.Format(
                        MachineDefinitionAggregateRootResources.AddTransition_TransitionAlreadyExists,
                        source.Name,
                        target.Name,
                        trigger));
            }

            var @event = TransitionAdded.Create(source, target, trigger);
            this.Enqueue(@event);
            this.Apply(@event);
        }

        public override void When(object @event)
        {
            switch (@event)
            {
                case MachineDefinitionCreated created:
                    this.Apply(created);
                    break;
                case MachineNodesAdded nodesAdded:
                    this.Apply(nodesAdded);
                    break;
                case TransitionAdded transitionAdded:
                    this.Apply(transitionAdded);
                    break;
            }

            base.When(@event);
        }

        private void Apply(MachineDefinitionCreated @event)
        {
            this.MachineName = @event.MachineName;
        }

        private void Apply(MachineNodesAdded @event)
        {
            foreach (var node in @event.Nodes)
            {
                this.nodes.Add(node);
            }
        }

        private void Apply(TransitionAdded @event)
        {
            this.transitions.Add(
                Transition.Create(
                    @event.Source,
                    @event.Target,
                    @event.Trigger));
        }
    }
}
