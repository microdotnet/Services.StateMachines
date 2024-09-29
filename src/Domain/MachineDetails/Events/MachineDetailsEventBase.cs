namespace MicroDotNet.Services.StateMachines.Domain.MachineDetails.Events
{
    public abstract class MachineDetailsEventBase
    {
        protected MachineDetailsEventBase(string code)
        {
            this.Code = code;
        }

        public string Code { get; }
    }
}
