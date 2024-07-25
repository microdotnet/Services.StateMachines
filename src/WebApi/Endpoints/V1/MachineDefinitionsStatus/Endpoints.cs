namespace MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitionsStatus;

using MicroDotNet.Services.StateMachines.WebApi.Endpoints.V1.MachineDefinitions;

public static class Endpoints
{
    public const string ConfirmMachineEndpointName = "V1_MachineDefinitionsStatus_Confirm";

    public static IResult Confirm(string code, short version)
    {
        var machine = Db.GetMachineDefinition(code, version);
        if (machine is null)
        {
            return Results.NotFound();
        }

        if (machine.Confirmed)
        {
            return Results.UnprocessableEntity();
        }

        Db.ConfirmMachine(machine.Id);
        machine = Db.GetMachineDefinition(code, version)!;

        var result = new MachineDefinition(
            machine.Id,
            machine.MachineCode,
            machine.MachineVersion,
            machine.Nodes,
            machine.Transitions,
            machine.Confirmed);
        return Results.Ok(result);
    }

    public static void MapEndpoints(WebApplication app)
    {
        app.MapPost("/v1/machineDefinitions/{code}/{version}/confirm", Confirm)
            .WithName(ConfirmMachineEndpointName)
            .WithOpenApi();
    }
}
