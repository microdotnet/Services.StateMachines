namespace MicroDotNet.Services.StateMachines.WebApi.TestClient.TestCases;

using System.Threading.Tasks;

public sealed class ConsoleOutputWriter : IOutputWriter
{
    private readonly object writeMessageLock = new();

    public Task WriteMessage(IOutputWriter.LogLevel level, string message, params object[] args)
    {
        lock (this.writeMessageLock)
        {
            var currentColor = Console.ForegroundColor;
            Console.ForegroundColor = GetOutputColor(level);
            Console.WriteLine(message, args);
            Console.ForegroundColor = currentColor;
            return Task.CompletedTask;
        }
    }

    private static ConsoleColor GetOutputColor(IOutputWriter.LogLevel logLevel)
    {
        return logLevel switch
        {
            IOutputWriter.LogLevel.Debug => ConsoleColor.Blue,
            IOutputWriter.LogLevel.Information => ConsoleColor.Green,
            IOutputWriter.LogLevel.Warning => ConsoleColor.Yellow,
            IOutputWriter.LogLevel.Error => ConsoleColor.Red,
            _ => throw new NotImplementedException()
        };
    }
}
