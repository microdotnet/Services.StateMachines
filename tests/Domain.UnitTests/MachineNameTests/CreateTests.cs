namespace MicroDotNet.Services.StateMachines.Domain.UnitTests.MachineNameTests;

using FluentAssertions;

using TestStack.BDDfy;

public sealed class CreateTests
{
    private MachineName? instanceUnderTests;

    private string? codeToSet;

    private short versionToSet;

    private Action? instanceCreation;

    [Fact]
    public void WhenCorrectParametersArePassedThenInstanceIsCreated()
    {
        this.Given(t => t.CodeIs("ValidMachine"))
            .And(t => t.VersionIs(123))
            .When(t => t.MachineNameIsCreated())
            .Then(t => t.ExceptionIsNotThrown())
            .And(t => t.MachineNameIsNotNull())
            .And(t => t.CodePropertyIs(this.codeToSet!))
            .And(t => t.VersionPropertyIs(this.versionToSet))
            .BDDfy();
    }

    [Fact]
    public void WhenEmptyCodeIsProvidedThenInstanceIsNotCreated()
    {
        this.Given(t => t.CodeIs("  "))
            .And(t => t.VersionIs(123))
            .When(t => t.MachineNameIsCreated())
            .Then(t => t.ExceptionIsThrown<ArgumentException>(e => e.ParamName == "code"))
            .And(t => t.MachineNameIsNull())
            .BDDfy();
    }

    [Fact]
    public void WhenNegativeVersionIsProvidedThenInstanceIsNotCreated()
    {
        this.Given(t => t.CodeIs("InvalidVersion"))
            .And(t => t.VersionIs(-1))
            .When(t => t.MachineNameIsCreated())
            .Then(t => t.ExceptionIsThrown<ArgumentException>(e => e.ParamName == "version"))
            .And(t => t.MachineNameIsNull())
            .BDDfy();
    }

    private void CodeIs(string value)
    {
        this.codeToSet = value;
    }

    private void VersionIs(short value)
    {
        this.versionToSet = value;
    }

    private void MachineNameIsCreated()
    {
        this.instanceCreation = () => this.instanceUnderTests = MachineName.Create(this.codeToSet!, this.versionToSet);
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

    private void MachineNameIsNull()
    {
        this.instanceUnderTests.Should()
            .BeNull();
    }

    private void MachineNameIsNotNull()
    {
        this.instanceUnderTests.Should()
            .NotBeNull();
    }

    private void CodePropertyIs(string value)
    {
        this.instanceUnderTests!.Code.Should().Be(value);
    }

    private void VersionPropertyIs(short value)
    {
        this.instanceUnderTests!.Version.Should().Be(value);
    }
}
