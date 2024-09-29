namespace MicroDotNet.Services.StateMachines.Application.ReadModel.MachineDefinitions
{
    public sealed class GetMachineDefinitionResponse
    {
        public GetMachineDefinitionResponse(MachineDefinition machine)
        {
            this.Machine = machine;
        }

        public MachineDefinition Machine { get; }
    }
}
