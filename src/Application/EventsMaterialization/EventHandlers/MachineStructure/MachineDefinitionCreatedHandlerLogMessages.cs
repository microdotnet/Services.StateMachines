namespace MicroDotNet.Services.StateMachines.Application.EventsMaterialization.EventHandlers.MachineStructure
{
    using System;

    using Microsoft.Extensions.Logging;

    public static class MachineDefinitionCreatedHandlerLogMessages
    {
        private static readonly Action<ILogger, string, short, Exception?> logMachineDefinitionCreated =
            LoggerMessage.Define<string, short>(
                LogLevel.Debug,
                new EventId(1, nameof(MachineDefinitionCreated)),
                "Machine definition '{Code} V{Version}' is being materialized.");

        public static void MachineDefinitionCreated(
            this ILogger<MachineDefinitionCreatedHandler> logger,
            string code,
            short version)
        {
            logMachineDefinitionCreated(logger, code, version, null);
        }
    }
}
