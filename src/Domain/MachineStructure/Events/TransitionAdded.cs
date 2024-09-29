namespace MicroDotNet.Services.StateMachines.Domain.MachineStructure.Events
{
    public sealed class TransitionAdded : MachineDefinitionEventBase
    {
        public TransitionAdded(
            MachineName machineName,
            Node source,
            Node target,
            string trigger)
            : base(machineName)
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
