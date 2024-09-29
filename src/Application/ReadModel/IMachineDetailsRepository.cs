namespace MicroDotNet.Services.StateMachines.Application.ReadModel
{
    using System.Threading;
    using System.Threading.Tasks;

    using MicroDotNet.Services.StateMachines.Application.ReadModel.MachineDetails;

    public interface IMachineDetailsRepository
    {
        public Task<CreateMachineResponse> CreateMachineAsync(
            CreateMachineRequest request,
            CancellationToken cancellationToken);

        public Task<GetMachineResponse> GetMachineAsync(
            GetMachineRequest request,
            CancellationToken cancellationToken);
    }
}
