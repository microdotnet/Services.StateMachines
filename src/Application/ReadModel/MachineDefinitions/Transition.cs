namespace MicroDotNet.Services.StateMachines.Application.ReadModel.MachineDefinitions
{
    public sealed class Transition
    {
        public Transition(string source, string target, string trigger)
        {
            this.Source = source;
            this.Target = target;
            this.Trigger = trigger;
        }

        public string Source { get; }

        public string Target { get; }
        
        public string Trigger { get; }
    }
}
