namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitions;

using System.Threading;
using System.Threading.Tasks;

public interface IMachineDefinitionsService
{
    Task<CreateOutput> CreateAsync(CreateInput input, CancellationToken cancellationToken);
}
