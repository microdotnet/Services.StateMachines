namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.MachineDefinitions;

public class MachineNode(
    string name)
{
    public string Name { get; } = name;
}
