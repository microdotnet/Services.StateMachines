namespace MicroDotNet.Services.StateMachines.Domain.MachineStructure.Events
{
    public sealed class NodesAdded : MachineDefinitionEventBase
    {
        public NodesAdded(
            MachineName machineName,
            Node[] nodes)
            : base(machineName)
        {
            this.Nodes = nodes;
        }

        public Node[] Nodes { get; }
    }
}
