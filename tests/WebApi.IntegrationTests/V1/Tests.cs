namespace MicroDotNet.Services.StateMachines.WebApi.IntegrationTests.V1;

using MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitions;

using Microsoft.AspNetCore.Mvc.Testing;

public sealed partial class Tests : TestClassBase, IClassFixture<CustomWebApplicationFactory<WebApiProgram>>
{
    private readonly CustomWebApplicationFactory<WebApiProgram> customWebApplicationFactory;

    private readonly HttpClient client;

    public Tests(CustomWebApplicationFactory<WebApiProgram> customWebApplicationFactory)
    {
        this.customWebApplicationFactory = customWebApplicationFactory;
        this.client = customWebApplicationFactory.CreateClient(new WebApplicationFactoryClientOptions()
        {
            AllowAutoRedirect = true,
        });
    }

    private static PostPayload CreateMachineDefinitionCreationPayload()
    {
        return new PostPayload(
             $"TestMachine_{DateTime.Now.Ticks}",
             1,
             [
                 new MachineNode("Node1"),
                 new MachineNode("Node2"),
                 new MachineNode("Node3"),
             ],
             [
                 new NodeTransition("Node1", "Node2"),
                 new NodeTransition("Node2", "Node3"),
             ]);
    }

    private static PutPayload CreateMachineDefinitionModificationPayload()
    {
        return new PutPayload(
            [
                new MachineNode("Node1-1"),
                new MachineNode("Node2-1"),
                new MachineNode("Node3-1"),
            ],
            [
                new NodeTransition("Node2-1", "Node1-1"),
                new NodeTransition("Node2-1", "Node3-1"),
            ]);
    }
}
