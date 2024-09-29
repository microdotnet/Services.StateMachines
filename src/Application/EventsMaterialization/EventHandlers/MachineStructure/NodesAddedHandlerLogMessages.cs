namespace MicroDotNet.Services.StateMachines.Application.EventsMaterialization.EventHandlers.MachineStructure
{
    using System;

    using Microsoft.Extensions.Logging;

    public static class NodesAddedHandlerLogMessages
    {
        private static readonly Action<ILogger, string, short, int, Exception?> logNodesAdded =
            LoggerMessage.Define<string, short, int>(
                LogLevel.Debug,
                new EventId(1, nameof(NodesAdded)),
                "{NumberOfNodes} added to machine definition '{Code} V{Version}' is being materialized.");

        public static void NodesAdded(
            this ILogger<NodesAddedHandler> logger,
            string code,
            short version,
            int numberOfNodes)
        {
            logNodesAdded(logger, code, version, numberOfNodes, null);
        }
    }
}
