namespace MicroDotNet.Services.StateMachines.Application.ReadModel.MachineDetails
{
    public sealed class UpdateMachineResponse
    {
        public UpdateMachineResponse(Result creationResult)
        {
            this.CreationResult = creationResult;
        }

        public Result CreationResult { get; }

        public enum Result
        {
            None = 0,

            Updated = 1,
        }
    }
}
