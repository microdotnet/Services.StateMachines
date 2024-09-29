namespace MicroDotNet.Services.StateMachines.Application.ReadModel
{
    using System.Threading;
    using System.Threading.Tasks;

    using MicroDotNet.Services.StateMachines.Application.ReadModel.MachineDefinitions;

    public interface IMachineDefinitionsRepository
    {
        Task<CreateMachineDefinitionResponse> CreateMachineDefinitionAsync(
            CreateMachineDefinitionRequest request,
            CancellationToken cancellationToken);

        Task<GetMachineDefinitionResponse> GetMachineDefinitionAsync(
            GetMachineDefinitionRequest request,
            CancellationToken cancellationToken);

        Task<UpdateMachineDefinitionResponse> UpdateMachineDefinitionAsync(
            UpdateMachineDefinitionRequest request,
            CancellationToken cancellationToken);
    }
}
