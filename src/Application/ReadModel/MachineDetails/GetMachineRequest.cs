namespace MicroDotNet.Services.StateMachines.Application.ReadModel.MachineDetails
{
    public sealed class GetMachineRequest
    {
        public GetMachineRequest(string code)
        {
            this.Code = code;
        }

        public string Code { get; }
    }
}
