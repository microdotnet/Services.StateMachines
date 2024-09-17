namespace MicroDotNet.Services.StateMachines.Domain
{
    using System;
    using System.Diagnostics;

    [DebuggerDisplay("{Code} - V{Version}")]
    public sealed class MachineName
    {
        public static readonly MachineName Empty = new MachineName(string.Empty, 0);

        private MachineName(string code, short version)
        {
            this.Code = code;
            this.Version = version;
        }

        public string Code { get; }

        public short Version { get; }

        public static MachineName Create(string code, short version)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException(
                    MachineNameResources.Create_CodeIsEmpty,
                    nameof(code));
            }

            if (version <= 0)
            {
                throw new ArgumentException(
                    MachineNameResources.Create_VersionIsNotPositive,
                    nameof(version));
            }

            return new MachineName(code, version);
        }
    }
}
