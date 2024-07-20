namespace MicroDotNet.Services.StateMachines.WebApi.TestClient.TestCases.V1;

using System.Threading;
using System.Threading.Tasks;

public sealed class CreateMachineDefinition : TestCaseBase<StateMachinesTestRunContext>
{
    public CreateMachineDefinition(IOutputWriter outputWriter)
        : base(outputWriter)
    {
    }

    protected override Task<TestRunResult> RunAsync(StateMachinesTestRunContext context, CancellationToken cancellationToken)
    {
        this.OutputWriter.WriteDebug("Running test {0}", context.RootUrl);
        this.OutputWriter.WriteInformation("Running test {0}", context.RootUrl);
        this.OutputWriter.WriteWarning("Running test {0}", context.RootUrl);
        this.OutputWriter.WriteError("Running test {0}", context.RootUrl);
        return Task.FromResult(new TestRunResult(TestRunResult.Result.Success));
    }
}
