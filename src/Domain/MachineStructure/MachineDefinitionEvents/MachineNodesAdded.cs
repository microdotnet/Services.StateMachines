namespace MicroDotNet.Services.StateMachines.Domain.MachineStructure.MachineDefinitionEvents
{
    public sealed class MachineNodesAdded : MachineDefinitionEventBase
    {
        public MachineNodesAdded(
            MachineName machineName,
            Node[] nodes)
            : base(machineName)
        {
            this.Nodes = nodes;
        }

        public Node[] Nodes { get; }
    }
}
