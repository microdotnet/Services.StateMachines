namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitionsVersionsNodes;

using OneOf;
using OneOf.Types;

public interface INodesService
{
    Activities Activities { get; }

    Metrics Metrics { get; }

    Task<OneOf<AddNodesOutput, NotFound, Error>> AddNodesAsync(
        string code,
        short version,
        AddNodesInput input,
        CancellationToken cancellationToken);
}
