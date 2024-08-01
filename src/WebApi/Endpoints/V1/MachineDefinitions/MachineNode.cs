namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitions;

public sealed class MachineNode
{
    public MachineNode(
        string name)
    {
        this.Name = name;
    }

    public string Name { get; }
}
