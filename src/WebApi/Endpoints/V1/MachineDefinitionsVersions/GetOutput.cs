namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitionsVersions;

using System.Collections.Generic;

public sealed class GetOutput
{
    public GetOutput(
        string machineName,
        short machineVersion,
        string status,
        List<Node> nodes,
        List<Transition> transitions)
    {
        this.MachineName = machineName;
        this.MachineVersion = machineVersion;
        this.Status = status;
        this.Nodes = nodes;
        this.Transitions = transitions;
    }

    public string MachineName { get; }

    public short MachineVersion { get; }

    public string Status { get; }

    public List<Node> Nodes { get; }

    public List<Transition> Transitions { get; }

    public class Node
    {
        public string Name { get; }

        public Node(string name)
        {
            this.Name = name;
        }
    }

    public class Transition
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
