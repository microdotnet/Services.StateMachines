namespace MicroDotNet.Services.StateMachines.WebApi.TestClient.TestCases;

public sealed class StateMachinesTestRunContext : TestRunContextBase
{
    public StateMachinesTestRunContext(string rootUrl)
    {
        this.RootUrl = rootUrl;
    }

    public string RootUrl { get; }
}
