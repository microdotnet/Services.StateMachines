namespace MicroDotNet.Services.StateMachines.Domain.UnitTests.MachineStructureTests.MachineDefinitionAggreagateRootTests
{
    using System.Collections.ObjectModel;
    using System.Linq.Expressions;

    using FluentAssertions;

    using MicroDotNet.Services.StateMachines.Domain.MachineStructure;
    using MicroDotNet.Services.StateMachines.Domain.MachineStructure.MachineDefinitionEvents;

    using TestStack.BDDfy;

    public sealed class AddTransitionTests
    {
        private MachineDefinitionAggregateRoot aggregate = default!;

        private List<object> eventsObserved = default!;

        private Action transitionAddingAction = default!;

        [Fact]
        public void WhenNodesAreAddedThenEventIsCreated()
        {
            Node[] nodes =
                [
                Node.Create("Node1"),
                Node.Create("Node2"),
                Node.Create("Node3"),
                ];
            this.Given(t => t.AggregateIsCreated())
                .And(t => t.NodesAreAdded(nodes))
                .When(t => t.TransitionIsAdded(nodes[0], nodes[1], "TRIGGER1"))
                .Then(t => t.ExceptionIsNotThrown())
                .And(t => t.EventIsAdded())
                .And(t => t.TransitionExistsOnEvent(nodes[0], nodes[1], "TRIGGER1"))
                .BDDfy();
        }

        [Fact]
        public void WhenSourceNodeDoesNotExistThenEventIsNotCreated()
        {
            Node[] nodes =
                [
                Node.Create("Node1"),
                Node.Create("Node2"),
                Node.Create("Node3"),
                ];
            this.Given(t => t.AggregateIsCreated())
                .And(t => t.NodesAreAdded(nodes))
                .When(t => t.TransitionIsAdded(Node.Create("Node4"), nodes[1], "TRIGGER1"))
                .Then(t => t.ExceptionIsThrown<InvalidOperationException>(e => !string.IsNullOrEmpty(e.Message)))
                .And(t => t.EventIsNotAdded())
                .BDDfy();
        }

        [Fact]
        public void WhenTargetNodeDoesNotExistThenEventIsNotCreated()
        {
            Node[] nodes =
                [
                Node.Create("Node1"),
                Node.Create("Node2"),
                Node.Create("Node3"),
                ];
            this.Given(t => t.AggregateIsCreated())
                .And(t => t.NodesAreAdded(nodes))
                .When(t => t.TransitionIsAdded(nodes[0], Node.Create("Node5"), "TRIGGER1"))
                .Then(t => t.ExceptionIsThrown<InvalidOperationException>(e => !string.IsNullOrEmpty(e.Message)))
                .And(t => t.EventIsNotAdded())
                .BDDfy();
        }

        private void AggregateIsCreated()
        {
            this.aggregate = MachineDefinitionAggregateRoot.Create(
                MachineName.Create("MACHINE_CODE", 1));
        }

        private void NodesAreAdded(Node[] nodes)
        {
            this.aggregate.AddNodes(nodes);
        }

        private void TransitionIsAdded(Node source, Node target, string trigger)
        {
            this.transitionAddingAction = () => this.aggregate.AddTransition(source, target, trigger);
        }

        private void ExceptionIsNotThrown()
        {
            this.transitionAddingAction.Should().NotThrow();
        }

        private void ExceptionIsThrown<TException>(Func<TException, bool> exceptionPredicate)
            where TException : Exception
        {
            this.transitionAddingAction.Should().Throw<TException>()
                .And.Should().Match(e => exceptionPredicate((TException)e));
        }

        private void EventIsAdded()
        {
            this.ReadEvents();
            this.eventsObserved.OfType<TransitionAdded>()
                .Should()
                .HaveCount(1);
        }

        private void EventIsNotAdded()
        {
            this.ReadEvents();
            this.eventsObserved.OfType<TransitionAdded>()
                .Should()
                .BeEmpty();
        }

        private void TransitionExistsOnEvent(Node source, Node target, string trigger)
        {
            this.ReadEvents();
            var transitionAddedEvent = this.eventsObserved
                .OfType<TransitionAdded>()
                .Single();
            transitionAddedEvent.Source.Should().Be(source);
            transitionAddedEvent.Target.Should().Be(target);
            transitionAddedEvent.Trigger.Should().Be(trigger);
        }

        private void ReadEvents()
        {
            if (this.eventsObserved != null)
            {
                return;
            }

            this.eventsObserved = this.aggregate.DequeueUncommittedEvents()
                .ToList();
        }
    }
}
