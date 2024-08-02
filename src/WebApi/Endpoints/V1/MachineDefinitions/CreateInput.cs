namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitions;

public record CreateInput(
    string Code,
    string Name,
    string Description);
