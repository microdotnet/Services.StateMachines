namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitionsVersions;

using OneOf;
using OneOf.Types;

public interface IVersionsService
{
    Activities Activities { get; }

    Metrics Metrics { get; }

    Task<OneOf<CreateOutput, NotFound>> CreateAsync(string code, CancellationToken cancellationToken);

    Task<OneOf<GetOutput, NotFound>> GetAsync(GetInput input, CancellationToken cancellationToken);

    Task<OneOf<AcceptOutput, NotFound, Error>> AcceptAsync(AcceptInput input, CancellationToken cancellationToken);
}
