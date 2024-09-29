namespace MicroDotNet.Services.StateMachines.Domain.MachineDetails.Events
{
    using System;

    public sealed class VersionAdded : MachineDetailsEventBase
    {
        public VersionAdded(string code, short versionNumber)
            : base(code)
        {
            this.VersionNumber = versionNumber;
        }

        public short VersionNumber { get; }
    }
}
