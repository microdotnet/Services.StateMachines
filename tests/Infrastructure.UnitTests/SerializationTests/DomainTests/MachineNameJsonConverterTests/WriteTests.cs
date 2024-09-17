namespace MicroDotNet.Services.StateMachines.Infrastructure.UnitTests.SerializationTests.DomainTests.MachineNameJsonConverterTests;

using System.Text.Json;

using FluentAssertions;

using MicroDotNet.Services.StateMachines.Domain;
using MicroDotNet.Services.StateMachines.Infrastructure.Serialization.Domain;

using TestStack.BDDfy;

public sealed class WriteTests
{
    private MachineName machineName = default!;

    private MachineNameJsonConverter converter = default!;

    private string serialized = default!;

    [Fact]
    public void WhenMachineIsSerializedThenSerializedValueIsCorrect()
    {
        this.Given(t => t.MachineNameIs("MACHINE", 2))
            .And(t => t.ConverterIsCreated())
            .When(t => t.MachineNameIsSerialized())
            .Then(t => t.SerializedValueIs("{\"code\":\"MACHINE\",\"version\":2}"))
            .BDDfy();
    }

    private void MachineNameIs(string code, short version)
    {
        this.machineName = MachineName.Create(code, version);
    }

    private void ConverterIsCreated()
    {
        this.converter = new();
    }

    private void MachineNameIsSerialized()
    {
        var options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
        options.Converters.Add(this.converter);
        this.serialized = JsonSerializer.Serialize(this.machineName, options);
    }

    private void SerializedValueIs(string expected)
    {
        this.serialized.Should()
            .Be(expected);
    }
}
