namespace MicroDotNet.Services.StateMachines.Application.ReadModel
{
    using System.Threading;
    using System.Threading.Tasks;

    using MicroDotNet.Services.StateMachines.Application.ReadModel.MachineDetails;

    using OneOf;
    using OneOf.Types;

    public interface IMachineDetailsRepository
    {
        public Task<OneOf<CreateMachineResponse, Error>> CreateMachineAsync(
            CreateMachineRequest request,
            CancellationToken cancellationToken);

        public Task<OneOf<GetMachineResponse, Error>> GetMachineAsync(
            GetMachineRequest request,
            CancellationToken cancellationToken);

        public Task<OneOf<UpdateMachineResponse, Error>> UpdateMachineAsync(
            UpdateMachineRequest request,
            CancellationToken cancellationToken);
    }
}
