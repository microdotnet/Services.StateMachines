namespace MicroDotNet.Services.StateMachines.Domain.MachineStructure.MachineDefinitionEvents
{
    public sealed class MachineNodesAdded
    {
        public MachineNodesAdded(Node[] nodes)
        {
            this.Nodes = nodes;
        }

        public Node[] Nodes { get; }
    }
}
