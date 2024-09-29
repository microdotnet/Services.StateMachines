namespace MicroDotNet.Services.StateMachines.Application.ReadModel.MachineDetails
{
    public sealed class CreateMachineResponse
    {
        public CreateMachineResponse(Result creationResult)
        {
            this.CreationResult = creationResult;
        }

        public Result CreationResult { get; }

        public enum Result
        {
            None = 0,

            Created = 1,
        }
    }
}
