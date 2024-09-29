namespace MicroDotNet.Services.StateMachines.Application.EventsMaterialization.EventHandlers.MachineDetails
{
    using Microsoft.Extensions.Logging;

    using System;

    public static class VersionAddedHandlerLogMessages
    {
        private static readonly Action<ILogger, string, short, Exception?> logMaterializingVersionAdded =
            LoggerMessage.Define<string, short>(
                LogLevel.Debug,
                new EventId(1, nameof(MaterializingVersionAdded)),
                "Materializing adding version '{Version}' to machine '{Code}'.");

        public static void MaterializingVersionAdded(
            this ILogger<VersionAddedHandler> logger,
            string code,
            short versionNumber)
        {
            logMaterializingVersionAdded(logger, code, versionNumber, null);
        }
    }
}
