namespace MicroDotNet.Services.StateMachines.Application.ReadModel.MachineDefinitions
{
    public sealed class GetMachineDefinitionRequest
    {
        public GetMachineDefinitionRequest(
            string code,
            short version)
        {
            this.Code = code;
            this.Version = version;
        }

        public string Code { get; }

        public short Version { get; }
    }
}
