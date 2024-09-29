namespace MicroDotNet.Services.StateMachines.Application.ReadModel.MachineDetails
{
    public sealed class UpdateMachineRequest
    {
        public UpdateMachineRequest(Machine machine)
        {
            this.Machine = machine;
        }

        public Machine Machine { get; }
    }
}
