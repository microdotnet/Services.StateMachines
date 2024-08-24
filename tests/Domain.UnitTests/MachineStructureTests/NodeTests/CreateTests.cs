namespace MicroDotNet.Services.StateMachines.Domain.UnitTests.MachineStructureTests.NodeTests;

using FluentAssertions;

using MicroDotNet.Services.StateMachines.Domain.MachineStructure;

using TestStack.BDDfy;

public sealed class CreateTests
{
    private Node? instanceUnderTests;

    private string? nodeNameToSet;

    private Action? instanceCreation;

    [Fact]
    public void WhenCorrectParametersArePassedThenInstanceIsCrated()
    {
        this.Given(t => t.NodeNameIs("ValidNodeName"))
            .When(t => t.NodeIsCreated())
            .Then(t => t.ExceptionIsNotThrown())
            .And(t => t.NodeIsNotNull())
            .And(t => t.NamePropertyIs(this.nodeNameToSet!))
            .BDDfy();
    }

    [Fact]
    public void WhenEmptyCodeIsProvidedThenInstanceIsNotCreated()
    {
        this.Given(t => t.NodeNameIs("  "))
            .When(t => t.NodeIsCreated())
            .Then(t => t.ExceptionIsThrown<ArgumentException>(e => e.ParamName == "name"))
            .And(t => t.NodeIsNull())
            .BDDfy();
    }

    private void NodeNameIs(string value)
    {
        this.nodeNameToSet = value;
    }

    private void NodeIsCreated()
    {
        this.instanceCreation = () => this.instanceUnderTests = Node.Create(this.nodeNameToSet!);
    }

    private void ExceptionIsNotThrown()
    {
        this.instanceCreation.Should().NotThrow();
    }

    private void ExceptionIsThrown<TException>(Func<TException, bool> exceptionPredicate)
        where TException : Exception
    {
        this.instanceCreation.Should().Throw<TException>()
            .And.Should().Match(e => exceptionPredicate((TException)e));
    }

    private void NodeIsNull()
    {
        this.instanceUnderTests.Should()
            .BeNull();
    }

    private void NodeIsNotNull()
    {
        this.instanceUnderTests.Should()
            .NotBeNull();
    }

    private void NamePropertyIs(string value)
    {
        this.instanceUnderTests!.Name.Should().Be(value);
    }
}
