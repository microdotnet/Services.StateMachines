namespace MicroDotNet.Services.StateMachines.Infrastructure.UnitTests.SerializationTests.DomainTests.MachineNameJsonConverterTests;

using System.Linq.Expressions;
using System.Text.Json;

using FluentAssertions;

using MicroDotNet.Services.StateMachines.Domain;
using MicroDotNet.Services.StateMachines.Infrastructure.Serialization.Domain;

using TestStack.BDDfy;

public sealed class ReadTests
{
    private string serialized = default!;

    private MachineNameJsonConverter converter = default!;

    private MachineName deserialized = default!;

    [Fact]
    public void WhenMachineNameIsDeserializedThenPropertiesHaveCorrectValues()
    {
        this.Given(t => t.SerializedValueIs("{\"code\":\"SOME_MACHINE\",\"version\":123}"))
            .And(t => t.ConverterIsCreated())
            .When(t => t.ValueIsDeserialized())
            .Then(t => t.DeserializedValueIs(v => v.Code == "SOME_MACHINE", "must have correct code"))
            .And(t => t.DeserializedValueIs(v => v.Version == 123, "must have correct version"))
            .BDDfy();
    }

    [Fact]
    public void WhenPropertiesOrderIsChangedThenPropertiesHaveCorrectValues()
    {
        this.Given(t => t.SerializedValueIs("{\"version\":123,\"code\":\"SOME_MACHINE\"}"))
            .And(t => t.ConverterIsCreated())
            .When(t => t.ValueIsDeserialized())
            .Then(t => t.DeserializedValueIs(v => v.Code == "SOME_MACHINE", "must have correct code"))
            .And(t => t.DeserializedValueIs(v => v.Version == 123, "must have correct version"))
            .BDDfy();
    }

    [Fact]
    public void WhenTestObjectIsDeserializedThenMachineNameIsCorrect()
    {
        this.Given(t => t.SerializedValueIs("{\"name\":{\"version\":123,\"code\":\"SOME_MACHINE\"}}"))
            .And(t => t.ConverterIsCreated())
            .When(t => t.TestObjectIsDeserialized())
            .Then(t => t.DeserializedValueIs(v => v.Code == "SOME_MACHINE", "must have correct code"))
            .And(t => t.DeserializedValueIs(v => v.Version == 123, "must have correct version"))
            .BDDfy();
    }

    private void SerializedValueIs(string value)
    {
        this.serialized = value;
    }

    private void ConverterIsCreated()
    {
        this.converter = new();
    }

    private void ValueIsDeserialized()
    {
        var options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
        options.Converters.Add(this.converter);
        this.deserialized = JsonSerializer.Deserialize<MachineName>(this.serialized, options)!;
    }

    private void TestObjectIsDeserialized()
    {
        var options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
        options.Converters.Add(this.converter);
        var test = JsonSerializer.Serialize(new TestObject() { Name = MachineName.Create("A", 2) }, options);
        System.Diagnostics.Debug.WriteLine(test);
        var temp = JsonSerializer.Deserialize<TestObject>(this.serialized, options)!;
        this.deserialized = temp.Name;
    }

    private void DeserializedValueIs(Expression<Func<MachineName, bool>> predicate, string message)
    {
        this.deserialized.Should()
            .Match(predicate, message);
    }

    public class TestObject
    {
        public MachineName Name { get; set; } = default!;
    }
}
