namespace MicroDotNet.Services.StateMachines.WebApi.TestClient.TestCases;

public class TestsResult
{
    public TestsResult(int testsRun, int failed, int succeeded)
    {
        this.TestsRun = testsRun;
        this.Failed = failed;
        this.Succeeded = succeeded;
    }

    public int TestsRun { get; }

    public int Failed { get; }

    public int Succeeded { get; }
}
