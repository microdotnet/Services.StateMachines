namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitions;

public record MachineDefinition(
        Guid Id,
        string MachineCode,
        short MachineVersion,
        MachineNode[] Nodes,
        NodeTransition[] Transitions);