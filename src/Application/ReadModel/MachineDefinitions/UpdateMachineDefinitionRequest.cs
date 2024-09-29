namespace MicroDotNet.Services.StateMachines.Application.ReadModel.MachineDefinitions
{
    public sealed class UpdateMachineDefinitionRequest
    {
        public UpdateMachineDefinitionRequest(MachineDefinition newMachineDefinition)
        {
            this.NewMachineDefinition = newMachineDefinition;
        }

        public MachineDefinition NewMachineDefinition { get; }
    }
}
