namespace MicroDotNet.Services.StateMachines.WebApi.TestClient.TestCases;

public interface IOutputWriter
{
    Task WriteMessage(
        LogLevel level,
        string message,
        params object[] args);

    public enum LogLevel
    {
        Debug = 0,
        Information = 1,
        Warning = 2,
        Error = 3,
    }
}
