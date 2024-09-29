namespace MicroDotNet.Services.StateMachines.Domain.MachineStructure
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using MicroDotNet.Services.StateMachines.Domain.MachineStructure.Events;

    public sealed class MachineDefinitionAggregateRoot : AggregateRootBase
    {
        private readonly Collection<Node> nodes = new Collection<Node>();

        private readonly Collection<Transition> transitions = new Collection<Transition>();

        public MachineDefinitionAggregateRoot()
        {
            this.MachineName = MachineName.Empty;
            this.Status = Status.InDesign;
        }

        private MachineDefinitionAggregateRoot(Guid id, MachineName machineName)
            : this()
        {
            var @event = new MachineDefinitionCreated(id, machineName);
            this.Enqueue(@event);
            this.Apply(@event);
        }

        public MachineName MachineName { get; private set; }

        public Status Status { get; private set; }

        public IReadOnlyCollection<Node> Nodes => this.nodes;

        public IReadOnlyCollection<Transition> Transitions => this.transitions;

        public override string PublicIdentifier => CreatePublicIdentifier(this.MachineName.Code, this.MachineName.Version);

        public static string CreatePublicIdentifier(string code, short version)
        {
            return $"{code}_{version}";
        }

        public static MachineDefinitionAggregateRoot Create(MachineName machineName)
        {
            if (machineName is null)
            {
                throw new ArgumentNullException(nameof(machineName));
            }

            return new MachineDefinitionAggregateRoot(Guid.NewGuid(), machineName);
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

            var @event = new NodesAdded(this.MachineName, nodes);
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

            var @event = new TransitionAdded(this.MachineName, source, target, trigger);
            this.Enqueue(@event);
            this.Apply(@event);
        }

        public void Confirm()
        {
            if (this.Status != Status.InDesign)
            {
                throw new InvalidOperationException(
                    MachineDefinitionAggregateRootResources.Confirm_NotInDesign);
            }

            var @event = new MachineConfirmed(this.MachineName);
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
                case NodesAdded nodesAdded:
                    this.Apply(nodesAdded);
                    break;
                case TransitionAdded transitionAdded:
                    this.Apply(transitionAdded);
                    break;
                case MachineConfirmed machineConfirmed:
                    this.Apply(machineConfirmed);
                    break;
            }

            base.When(@event);
        }

        private void Apply(MachineDefinitionCreated @event)
        {
            this.Id = @event.Id;
            this.MachineName = @event.MachineName;
        }

        private void Apply(NodesAdded @event)
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

        private void Apply(MachineConfirmed @event)
        {
            this.Status = Status.Completed;
        }
    }
}
