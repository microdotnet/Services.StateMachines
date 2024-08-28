namespace MicroDotNet.Services.StateMachines.Domain.MachineStructure.MachineDefinitionEvents
{
    public sealed class MachineNodesAdded
    {
        private MachineNodesAdded(Node[] nodes)
        {
            this.Nodes = nodes;
        }

        public Node[] Nodes { get; }

        public static MachineNodesAdded Create(Node[] nodes)
        {
            return new MachineNodesAdded(nodes);
        }
    }
}
