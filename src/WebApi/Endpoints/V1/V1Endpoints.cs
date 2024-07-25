﻿namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1;

using System;

using MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitions;

public static class V1Endpoints
{
    private static readonly Dictionary<Guid, MachineData> machines = [];

    public static IResult Get(
        string code,
        short version,
        LinkGenerator linkGenerator)
    {
        var machine = machines.Values.FirstOrDefault(d => d.MachineCode == code && d.MachineVersion == version);
        if (machine is null)
        {
            return Results.NotFound();
        }

        var result = new MachineDefinition(
            machine.Id,
            machine.MachineCode,
            machine.MachineVersion,
            machine.Nodes,
            machine.Transitions);
        return Results.Ok(result);
    }

    public static IResult Post(
        LinkGenerator linkGenerator,
        PostPayload payload)
    {
        var machine = FindMachine(payload.Code, payload.Version);
        if (machine is not null)
        {
            return Results.BadRequest();
        }

        var id = Guid.NewGuid();
        var definitionToStore = new MachineData(
            id,
            payload.Code,
            payload.Version,
            payload.Nodes,
            payload.Transitions);
        machines.Add(id, definitionToStore);
        var link = linkGenerator.GetPathByName(
            "V1_GetMachineDefinition",
            values: new { code = payload.Code, version = payload.Version });

        var result = new MachineDefinition(
            id,
            definitionToStore.MachineCode,
            definitionToStore.MachineVersion,
            definitionToStore.Nodes,
            definitionToStore.Transitions);
        return Results.Created(link, result);
    }

    public static IResult Put(
        LinkGenerator linkGenerator,
        string code,
        short version,
        PutPayload payload)
    {
        var machine = FindMachine(code, version);
        if (machine is null)
        {
            return Results.NotFound();
        }

        var id = machine.Id;
        var definitionToStore = new MachineData(
            id,
            code,
            version,
            payload.Nodes,
            payload.Transitions);
        machines[id] = definitionToStore;
        var link = linkGenerator.GetPathByName(
            "V1_GetMachineDefinition",
            values: new { code, version });
        var result = new MachineDefinition(
           definitionToStore.Id,
           definitionToStore.MachineCode,
           definitionToStore.MachineVersion,
           definitionToStore.Nodes,
           definitionToStore.Transitions);
        return Results.Accepted(link, result);
    }

    public static WebApplication MapV1Endpoints(this WebApplication app)
    {
        app.MapMachineDefinitionsEndpoints();
        return app;
    }

    private static WebApplication MapMachineDefinitionsEndpoints(this WebApplication app)
    {
        app.MapPost("/v1/machineDefinitions", Post)
            .WithName("V1_CreateMachineDefinition")
            .WithOpenApi();
        app.MapGet("/v1/machineDefinitions/{code}/{version}", Get)
            .WithName("V1_GetMachineDefinition")
            .WithOpenApi();
        app.MapPut("/v1/machineDefinitions/{code}/{version}", Put)
            .WithName("V1_UpdateMachineDefinition")
            .WithOpenApi();
        return app;
    }

    private static MachineData? FindMachine(string code, short version)
    {
        return machines.Values.FirstOrDefault(m => m.MachineCode == code && m.MachineVersion == version);
    }

    private class MachineData(
        Guid id,
        string machineCode,
        short machineVersion,
        MachineNode[] nodes,
        NodeTransition[] transitions)
    {
        public Guid Id { get; } = id;

        public string MachineCode { get; } = machineCode;

        public short MachineVersion { get; } = machineVersion;

        public MachineNode[] Nodes { get; } = nodes;

        public NodeTransition[] Transitions { get; } = transitions;
    }
}
