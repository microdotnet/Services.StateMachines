namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitions;

using System.Threading;
using System.Threading.Tasks;

using OneOf;
using OneOf.Types;

public interface IMachineDefinitionsService
{
    Task<CreateOutput> CreateAsync(CreateInput input, CancellationToken cancellationToken);

    Task<OneOf<GetOutput, NotFound>> GetAsync(string code, CancellationToken cancellationToken);
}
