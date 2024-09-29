namespace MicroDotNet.Services.StateMachines.Domain.UnitTests.MachineStructureTests.MachineDefinitionAggreagateRootTests
{
    using System.Collections.ObjectModel;
    using System.Linq.Expressions;

    using FluentAssertions;

    using MicroDotNet.Services.StateMachines.Domain.MachineStructure;
    using MicroDotNet.Services.StateMachines.Domain.MachineStructure.Events;

    using TestStack.BDDfy;

    public sealed class AddNodesTests
    {
        private readonly Collection<Node> nodes = [];

        private MachineDefinitionAggregateRoot aggregate = default!;

        private List<object> eventsObserved = default!;

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
                .And(t => t.WithNode(nodes[0]))
                .And(t => t.WithNode(nodes[1]))
                .And(t => t.WithNode(nodes[2]))
                .When(t => t.NodesAreAdded())
                .Then(t => t.EventIsAdded())
                .And(t => t.NodeExistsOnEvent(e => ReferenceEquals(e, nodes[0]), "node 1 should be added"))
                .And(t => t.NodeExistsOnEvent(e => ReferenceEquals(e, nodes[1]), "node 2 should be added"))
                .And(t => t.NodeExistsOnEvent(e => ReferenceEquals(e, nodes[2]), "node 3 should be added"))
                .BDDfy();
        }

        private void AggregateIsCreated()
        {
            this.aggregate = MachineDefinitionAggregateRoot.Create(
                MachineName.Create("MACHINE_CODE", 1));
        }

        private void WithNode(Node value)
        {
            this.nodes.Add(value);
        }

        private void NodesAreAdded()
        {
            this.aggregate.AddNodes([.. this.nodes]);
        }

        private void EventIsAdded()
        {
            this.ReadEvents();
            this.eventsObserved.OfType<NodesAdded>()
                .Should()
                .NotBeEmpty()
                .And.HaveCount(1);
        }

        private void NodeExistsOnEvent(Expression<Func<Node, bool>> nodeFinder, string message)
        {
            this.ReadEvents();
            var nodesAddedEvent = this.eventsObserved
                .OfType<NodesAdded>()
                .Single();
            nodesAddedEvent.Nodes.Should()
                .Contain(nodeFinder, message);
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
