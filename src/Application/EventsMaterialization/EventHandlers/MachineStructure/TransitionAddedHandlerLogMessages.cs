namespace MicroDotNet.Services.StateMachines.Application.EventsMaterialization.EventHandlers.MachineStructure
{
    using System;

    using MicroDotNet.Services.StateMachines.Domain.MachineStructure;

    using Microsoft.Extensions.Logging;

    public static class TransitionAddedHandlerLogMessages
    {
        private static readonly Action<ILogger, string, short, Node, Node, string, Exception?> logTransitionAdded =
            LoggerMessage.Define<string, short, Node, Node, string>(
                LogLevel.Debug,
                new EventId(1, nameof(TransitionAdded)),
                "Transition from '{Source}' to '{Target}' on '{Trigger}' added to machine definition '{Code} V{Version}' is being materialized.");

        public static void TransitionAdded(
            this ILogger<TransitionAddedHandler> logger,
            string code,
            short version,
            Node source,
            Node target,
            string trigger)
        {
            logTransitionAdded(logger, code, version, source, target, trigger, null);
        }
    }
}
