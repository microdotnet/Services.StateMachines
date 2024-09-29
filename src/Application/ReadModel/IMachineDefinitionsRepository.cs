namespace MicroDotNet.Services.StateMachines.Application.ReadModel
{
    using System.Threading;
    using System.Threading.Tasks;

    using MicroDotNet.Services.StateMachines.Application.ReadModel.MachineDefinitions;

    using OneOf;
    using OneOf.Types;

    public interface IMachineDefinitionsRepository
    {
        Task<OneOf<CreateMachineDefinitionResponse, Error>> CreateMachineDefinitionAsync(
            CreateMachineDefinitionRequest request,
            CancellationToken cancellationToken);

        Task<OneOf<GetMachineDefinitionResponse, Error>> GetMachineDefinitionAsync(
            GetMachineDefinitionRequest request,
            CancellationToken cancellationToken);

        Task<OneOf<UpdateMachineDefinitionResponse, Error>> UpdateMachineDefinitionAsync(
            UpdateMachineDefinitionRequest request,
            CancellationToken cancellationToken);
    }
}
