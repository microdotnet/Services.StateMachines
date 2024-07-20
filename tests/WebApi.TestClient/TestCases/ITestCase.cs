namespace MicroDotNet.Services.StateMachines.WebApi.TestClient.TestCases;

public interface ITestCase
{
    Task<TestRunResult> RunAsync(TestRunContextBase context, CancellationToken cancellationToken);
}
