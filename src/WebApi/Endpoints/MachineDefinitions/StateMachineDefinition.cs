namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.MachineDefinitions;

public class StateMachineDefinition(
    Guid id,
    string machineCode,
    short machineVersion)
{
    public Guid Id { get; } = id;

    public string MachineCode { get; } = machineCode;

    public short MachineVersion { get; } = machineVersion;
}