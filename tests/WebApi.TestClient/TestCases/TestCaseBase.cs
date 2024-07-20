namespace MicroDotNet.Services.StateMachines.WebApi.TestClient.TestCases;

public abstract class TestCaseBase<TContext> : ITestCase
    where TContext : TestRunContextBase
{
    private readonly IOutputWriter outputWriter;

    protected TestCaseBase(IOutputWriter outputWriter)
    {
        ArgumentNullException.ThrowIfNull(outputWriter);
        this.outputWriter = outputWriter;
    }

    protected IOutputWriter OutputWriter => this.outputWriter;

    public async Task<TestRunResult> RunAsync(TestRunContextBase context, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (context is not TContext contextCast)
        {
            throw new ArgumentException(
                $"Context must be a subtype of {nameof(TContext)} and not null",
                nameof(context));
        }

        return await this.RunAsync(contextCast, cancellationToken)
            .ConfigureAwait(false);
    }

    protected abstract Task<TestRunResult> RunAsync(
        TContext context,
        CancellationToken cancellationToken);
}
