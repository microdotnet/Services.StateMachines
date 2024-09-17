namespace MicroDotNet.Services.StateMachines.Domain.MachineStructure.MachineDefinitionEvents
{
    public sealed class TransitionAdded
    {
        public TransitionAdded(
            Node source,
            Node target,
            string trigger)
        {
            this.Source = source;
            this.Target = target;
            this.Trigger = trigger;
        }


        public Node Source { get; }

        public Node Target { get; }

        public string Trigger { get; }
    }
}
