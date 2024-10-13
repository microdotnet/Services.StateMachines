namespace MicroDotNet.Services.StateMachines.Infrastructure.AggregatesManagers.EventStoreDb;

using System.Diagnostics;

using MicroDotNet.Services.StateMachines.Domain;

public sealed class EventStoreActivities
{
    public const string ActivityName = "MicroDotNet.StateMachines.EventStore.AggregatesRepository";

    private const string AddAggregate = nameof(AddAggregate);

    private readonly ActivitySource activitySource;

    public EventStoreActivities()
    {
        this.activitySource = new ActivitySource(ActivityName);
    }

    public Activity? StartAddingAggregate<TAggregate>(
        TAggregate aggregate)
        where TAggregate : IAggregate
    {
        var activity = this.activitySource.StartActivity(
            AddAggregate);
        if (activity is null)
        {
            return activity;
        }

        activity.SetTag("AggregateType", typeof(TAggregate).FullName);
        activity.SetTag("AggregateId", aggregate.PublicIdentifier);
        return activity;
    }
}
