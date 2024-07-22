namespace MicroDotNet.Services.StateMachines.WebApi.TestClient.TestCases;

using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;

public sealed class TestsRunner : BackgroundService
{
    private readonly IHostApplicationLifetime hostLifetime;

    private readonly IList<ITestCase> testCases;

    public TestsRunner(
        IHostApplicationLifetime hostLifetime,
        IEnumerable<ITestCase> testCases)
    {
        ArgumentNullException.ThrowIfNull(testCases);
        this.testCases = testCases
            .ToList();
        this.hostLifetime = hostLifetime;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        TestRunContextBase context = new StateMachinesTestRunContext(
            "https://localhost:7030");
        var resultFactory = new TestsResultFactory();
        Console.WriteLine("Prepared to run tests. Press ENTER to execute tests");
        Console.ReadLine();
        foreach (var testCase in this.testCases)
        {
            var testResult = await testCase.RunAsync(context, stoppingToken)
                .ConfigureAwait(false);
            resultFactory.AddTestRun(testResult);
        }

        var testRunResult = resultFactory.Create();
        var currentColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Test run finished");
        Console.WriteLine($"Tests run: {testRunResult.TestsRun}");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Tests succeeded: {testRunResult.Succeeded}");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Tests failed: {testRunResult.Failed}");
        Console.ForegroundColor = currentColor;
        Console.WriteLine("Press ENTER to exit");
        Console.ReadLine();
        this.hostLifetime.StopApplication();
    }
}
