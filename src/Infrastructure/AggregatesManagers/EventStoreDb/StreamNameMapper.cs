﻿namespace MicroDotNet.Services.StateMachines.Infrastructure.AggregatesManagers.EventStoreDb;

using System;
using System.Collections.Concurrent;
using System.Linq;

public sealed class StreamNameMapper
{
    private static readonly StreamNameMapper Instance = new();

    private readonly ConcurrentDictionary<Type, string> typeNameMap = [];

    public static void AddCustomMap<TStream>(string mappedStreamName) =>
        AddCustomMap(typeof(TStream), mappedStreamName);

    public static void AddCustomMap(Type streamType, string mappedStreamName)
    {
        Instance.typeNameMap.AddOrUpdate(streamType, mappedStreamName, (_, _) => mappedStreamName);
    }

    public static string ToStreamPrefix<TStream>() => ToStreamPrefix(typeof(TStream));

    public static string ToStreamPrefix(Type streamType) => Instance.typeNameMap.GetOrAdd(streamType, _ =>
    {
        var modulePrefix = streamType.Namespace!.Split(".").First();
        return $"{modulePrefix}_{streamType.Name}";
    });

    public static string ToStreamId<TStream>(object aggregateId, object? tenantId = null) =>
        ToStreamId(typeof(TStream), aggregateId, tenantId);

    public static string ToStreamId(Type streamType, object aggregateId, object? tenantId = null)
    {
        var tenantPrefix = tenantId != null ? $"{tenantId}_" : string.Empty;
        var streamCategory = ToStreamPrefix(streamType);

        // (Out-of-the box, the category projection treats anything before a `-` separator as the category name)
        // For this reason, we place the "{tenantId}_" bit (if present) on the right hand side of the '-'
        return $"{streamCategory}-{tenantPrefix}{aggregateId}";
    }
}