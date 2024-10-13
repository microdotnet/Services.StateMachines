namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitions;

using System.Diagnostics;

public sealed class MachineDefinitionsActivities
{
    public const string ActivityName = "MicroDotNet.StateMachines.Api.MachineDefintitions";

    private const string MachineCreation = nameof(MachineCreation);

    private readonly ActivitySource activitySource;

    public MachineDefinitionsActivities()
    {
        this.activitySource = new ActivitySource(ActivityName);
    }

    public Activity? StartMachineCreation()
    {
        return this.activitySource.StartActivity(MachineCreation);
    }
}
