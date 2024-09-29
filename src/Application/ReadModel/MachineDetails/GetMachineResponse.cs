namespace MicroDotNet.Services.StateMachines.Application.ReadModel.MachineDetails
{
    using System;

    public sealed class GetMachineResponse
    {
        public GetMachineResponse(Machine machineDetails)
        {
            this.MachineDetails = machineDetails;
        }

        public Machine MachineDetails { get; }
    }
}
