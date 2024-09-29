namespace MicroDotNet.Services.StateMachines.Application.EventsMaterialization.EventHandlers.MachineDetails
{
    using System;

    using Microsoft.Extensions.Logging;

    public static partial class MachineDetailCreatedHandlerLogMessages
    {
        private static readonly Action<ILogger, string, Exception?> logMaterializingMachine =
            LoggerMessage.Define<string>(
                LogLevel.Debug,
                new EventId(1, nameof(MaterializingMachine)),
                "Materializing machine '{Code}'.");

        public static void MaterializingMachine(
            this ILogger<MachineDetailCreatedHandler> logger,
            string code){
            logMaterializingMachine(logger, code, null);
        }
    }
}
