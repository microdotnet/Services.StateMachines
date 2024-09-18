namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitionsVersionsTransitions;

using System.Collections.ObjectModel;

public record AddTransitionsInput(
    Collection<AddTransitionsInput.Transition> Transitions)
{
    public record Transition(
        string SourceNode,
        string TargetNode,
        string TriggerName);
}
