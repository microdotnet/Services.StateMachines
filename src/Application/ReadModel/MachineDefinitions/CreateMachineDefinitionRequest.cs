namespace MicroDotNet.Services.StateMachines.Application.ReadModel.MachineDefinitions
{
    public sealed class CreateMachineDefinitionRequest
    {
        public CreateMachineDefinitionRequest(MachineDefinition machine)
        {
            this.Machine = machine;
        }

        public MachineDefinition Machine { get; }
    }
}
