namespace MicroDotNet.Services.StateMachines.Application.ReadModel.MachineDetails
{
    using System;

    public sealed class GetMachineResponse
    {
        private Machine? machineDetails;

        private GetMachineResponse(Machine? machineDetails)
        {
            this.machineDetails = machineDetails;
        }

        public Machine MachineDetails
        {
            get
            {
                if (this.machineDetails is null)
                {
                    throw new InvalidOperationException();
                }

                return this.machineDetails;
            }
        }

        public bool IsFound => !(this.machineDetails is null);

        public static GetMachineResponse Found(Machine machine) => new GetMachineResponse(machine);

        public static GetMachineResponse NotFound() => new GetMachineResponse(null);
    }
}
