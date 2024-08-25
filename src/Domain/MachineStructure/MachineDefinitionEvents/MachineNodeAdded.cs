namespace MicroDotNet.Services.StateMachines.Domain.MachineStructure.MachineDefinitionEvents
{
    public sealed class MachineNodeAdded
    {
        private MachineNodeAdded(Node node)
        {
            this.Node = node;
        }

        public Node Node { get; }

        public static MachineNodeAdded Create(Node node)
        {
            return new MachineNodeAdded(node);
        }
    }
}
