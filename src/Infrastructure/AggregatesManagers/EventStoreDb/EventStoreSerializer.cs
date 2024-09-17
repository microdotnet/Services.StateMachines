namespace MicroDotNet.Services.StateMachines.Infrastructure.AggregatesManagers.EventStoreDb;

using System.Text;
using System.Text.Json;

using EventStore.Client;

public static class EventStoreSerializer
{
    private static readonly JsonSerializerOptions SerializerSettings;

    static EventStoreSerializer()
    {
        SerializerSettings = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
        SerializerSettings.Converters.Add(new Serialization.Domain.MachineNameJsonConverter());
    }

    public static T? Deserialize<T>(this ResolvedEvent resolvedEvent)
        where T : class =>
        resolvedEvent.Deserialize() as T;

    public static object? Deserialize(this ResolvedEvent resolvedEvent)
    {
        // get type
        var eventType = EventTypeMapper.Instance.ToType(resolvedEvent.Event.EventType);

        if (eventType == null)
        {
            return null;
        }

        // deserialize event
        return JsonSerializer.Deserialize(
            Encoding.UTF8.GetString(resolvedEvent.Event.Data.Span),
            eventType,
            SerializerSettings)!;
    }

    public static EventData ToJsonEventData(this object @event, object? metadata = null) =>
        new(
            Uuid.NewUuid(),
            EventTypeMapper.Instance.ToName(@event.GetType()),
            Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event, SerializerSettings)),
            Encoding.UTF8.GetBytes(JsonSerializer.Serialize(metadata ?? new { }, SerializerSettings)));
}