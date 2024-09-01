namespace MicroDotNet.Services.StateMachines.Infrastructure.AggregatesManagers.EventStoreDb;

using System.Collections.Concurrent;

public sealed class EventTypeMapper
{
    public static readonly EventTypeMapper Instance = new();

    private readonly ConcurrentDictionary<string, Type?> typeMap = new();

    private readonly ConcurrentDictionary<Type, string> typeNameMap = new();

    public void AddCustomMap<T>(string eventTypeName) => this.AddCustomMap(typeof(T), eventTypeName);

    public void AddCustomMap(Type eventType, string eventTypeName)
    {
        this.typeNameMap.AddOrUpdate(eventType, eventTypeName, (_, typeName) => typeName);
        this.typeMap.AddOrUpdate(eventTypeName, eventType, (_, type) => type);
    }

    public string ToName<TEventType>()
        where TEventType : class
        => this.ToName(typeof(TEventType));

    public string ToName(Type eventType) => this.typeNameMap.GetOrAdd(eventType, _ =>
    {
        var eventTypeName = eventType.FullName!;

        this.typeMap.TryAdd(eventTypeName, eventType);

        return eventTypeName;
    });

    public Type? ToType(string eventTypeName)
    {
        return this.typeMap.GetOrAdd(eventTypeName, _ =>
        {
            var type = TypeProvider.GetFirstMatchingTypeFromCurrentDomainAssembly(eventTypeName);

            if (type == null)
            {
                return null;
            }

            this.typeNameMap.TryAdd(type, eventTypeName);

            return type;
        });
    }
}
