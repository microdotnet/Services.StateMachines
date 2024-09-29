namespace MicroDotNet.Services.StateMachines.Application.ReadModel.MachineDetails
{
    public sealed class CreateMachineRequest
    {
        public CreateMachineRequest(Machine machine)
        {
            this.Machine = machine;
        }

        public Machine Machine { get; }
    }
}
