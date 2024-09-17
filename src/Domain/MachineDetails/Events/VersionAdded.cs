namespace MicroDotNet.Services.StateMachines.Domain.MachineDetails.Events
{
    using System;

    public sealed class VersionAdded
    {
        public VersionAdded(short versionNumber)
        {
            this.VersionNumber = versionNumber;
        }

        public short VersionNumber { get; }
    }
}
