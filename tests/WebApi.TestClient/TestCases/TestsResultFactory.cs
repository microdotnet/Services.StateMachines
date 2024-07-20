namespace MicroDotNet.Services.StateMachines.WebApi.TestClient.TestCases;

public class TestsResultFactory
{
    private readonly object lockObject = new();

    private int testsRun;

    private int failed;

    private int succeeded;

    public TestsResult Create()
    {
        return new(this.testsRun, this.failed, this.succeeded);
    }

    public TestsResultFactory AddTestRun(TestRunResult testRun)
    {
        lock (this.lockObject)
        {
            this.testsRun++;
            switch (testRun.RunResult)
            {
                case TestRunResult.Result.Success:
                    this.succeeded++;
                    break;
                case TestRunResult.Result.Failure:
                    this.failed++;
                    break;
            }
        }

        return this;
    }
}
