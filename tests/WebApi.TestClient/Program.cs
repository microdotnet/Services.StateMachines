using MicroDotNet.Services.StateMachines.WebApi.TestClient.TestCases;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

CreateHostBuilder(args).Build().Run();

static IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            services.AddSingleton<IOutputWriter, ConsoleOutputWriter>();
            services.AddTransient<ITestCase, MicroDotNet.Services.StateMachines.WebApi.TestClient.TestCases.V1.WhenMachineDefinitionIsCreatedThenSuccessIsReturned>();
            services.AddHostedService<TestsRunner>();
        });
}