namespace MicroDotNet.Services.StateMachines.WebApi.TestClient.TestCases;

public class TestRunResult
{
    public TestRunResult(Result runResult)
    {
        this.RunResult = runResult;
    }

    public Result RunResult { get; }

    public enum Result
    {

        Success,
        Failure,
    }
}
