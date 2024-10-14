namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitionsVersionsNodes;

using System.Collections.ObjectModel;

public record AddNodesInput(
    Collection<AddNodesInput.Node> Nodes)
{
    public record Node(string Name);
}
