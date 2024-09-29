namespace MicroDotNet.Services.StateMachines.Application.EventsMaterialization.EventHandlers.MachineStructure
{
    using System;

    using MicroDotNet.Services.StateMachines.Domain.MachineStructure;

    using Microsoft.Extensions.Logging;

    public static class MachineConfirmedHandlerLogMessages
    {
        private static readonly Action<ILogger, string, short, Exception?> logMachineConfirmed =
            LoggerMessage.Define<string, short>(
                LogLevel.Debug,
                new EventId(1, nameof(MachineConfirmed)),
                "Machine definition '{Code} V{Version}' is being confirmed.");

        public static void MachineConfirmed(
            this ILogger<MachineConfirmedHandler> logger,
            string code,
            short version)
        {
            logMachineConfirmed(logger, code, version, null);
        }
    }
}
