namespace MicroDotNet.Services.StateMachines.WebApi.TestClient.TestCases;

using System.Collections.ObjectModel;

using Humanizer;

public abstract class StateMachinesTestCaseBase : TestCaseBase<StateMachinesTestRunContext>
{
    protected StateMachinesTestCaseBase(IOutputWriter outputWriter)
        : base(outputWriter)
    {
    }

    protected override string TestCaseName
    {
        get
        {
            var testCaseType = this.GetType();
            var segments = new Collection<string>();
            segments.Add(testCaseType.Namespace!.Split('.')[^1]);
            segments.Add(testCaseType.Name.Humanize());
            return string.Join(" - ", segments);
        }
    }
}
