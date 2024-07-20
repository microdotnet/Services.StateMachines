namespace MicroDotNet.Services.StateMachines.WebApi.TestClient.TestCases;

public static class OutputWriterExtensions
{
    public static Task WriteDebug(
        this IOutputWriter outputWriter,
        string message,
        params object[] args)
    {
        return outputWriter.WriteMessage(IOutputWriter.LogLevel.Debug, message, args);
    }

    public static Task WriteInformation(
        this IOutputWriter outputWriter,
        string message,
        params object[] args)
    {
        return outputWriter.WriteMessage(IOutputWriter.LogLevel.Information, message, args);
    }

    public static Task WriteWarning(
        this IOutputWriter outputWriter,
        string message,
        params object[] args)
    {
        return outputWriter.WriteMessage(IOutputWriter.LogLevel.Warning, message, args);
    }

    public static Task WriteError(
        this IOutputWriter outputWriter,
        string message,
        params object[] args)
    {
        return outputWriter.WriteMessage(IOutputWriter.LogLevel.Error, message, args);
    }
}
