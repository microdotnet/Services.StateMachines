namespace MicroDotNet.Services.StateMachines.Domain.UnitTests.MachineStructureTests.TransitionTests;

using FluentAssertions;

using MicroDotNet.Services.StateMachines.Domain.MachineStructure;

using TestStack.BDDfy;

public sealed class CreateTests
{
    private Transition? instanceUnderTests;

    private string? sourceNode;

    private string? targetNode;

    private string? trigger;

    private Action? instanceCreation;

    [Fact]
    public void WhenCorrectParametersArePassedThenInstanceIsCreated()
    {
        this.Given(t => t.SourceIs("SOURCE"))
            .And(t => t.TargetNodeIs("TARGET"))
            .And(t => t.TriggerIs("TRIGGER"))
            .When(t => t.TransitionIsCreated())
            .Then(t => t.ExceptionIsNotThrown())
            .And(t => t.TransitionIsNotNull())
            .And(t => t.SourcePropertyIs(this.sourceNode!))
            .And(t => t.TargetPropertyIs(this.targetNode!))
            .And(t => t.TriggerPropertyIs(this.trigger!))
            .BDDfy();
    }

    [Fact]
    public void WhenEmptyTargetIsProvidedThenInstanceIsNotCreated()
    {
        this.Given(t => t.SourceIs("  "))
            .And(t => t.TargetNodeIs("TARGET"))
            .And(t => t.TriggerIs("TRIGGER"))
            .When(t => t.TransitionIsCreated())
            .Then(t => t.ExceptionIsThrown<ArgumentException>(e => e.ParamName == "source"))
            .And(t => t.TransitionIsNull())
            .BDDfy();
    }

    [Fact]
    public void WhenEmptyTriggerIsProvidedThenInstanceIsNotCreated()
    {
        this.Given(t => t.SourceIs("SOURCE"))
            .And(t => t.TargetNodeIs("   "))
            .And(t => t.TriggerIs("TRIGGER"))
            .When(t => t.TransitionIsCreated())
            .Then(t => t.ExceptionIsThrown<ArgumentException>(e => e.ParamName == "target"))
            .And(t => t.TransitionIsNull())
            .BDDfy();
    }

    [Fact]
    public void WhenEmptySourceIsProvidedThenInstanceIsNotCreated()
    {
        this.Given(t => t.SourceIs("SOURCE"))
            .And(t => t.TargetNodeIs("TARGET"))
            .And(t => t.TriggerIs("   "))
            .When(t => t.TransitionIsCreated())
            .Then(t => t.ExceptionIsThrown<ArgumentException>(e => e.ParamName == "trigger"))
            .And(t => t.TransitionIsNull())
            .BDDfy();
    }

    private void SourceIs(string value)
    {
        this.sourceNode = value;
    }

    private void TargetNodeIs(string value)
    {
        this.targetNode = value;
    }

    private void TriggerIs(string value)
    {
        this.trigger = value;
    }

    private void TransitionIsCreated()
    {
        Node? source = null;
        if (!string.IsNullOrWhiteSpace(this.sourceNode))
        {
            source = Node.Create(this.sourceNode);
        }

        Node? target = null;
        if (!string.IsNullOrWhiteSpace(this.targetNode))
        {
            target = Node.Create(this.targetNode);
        }

        this.instanceCreation = () => this.instanceUnderTests = Transition.Create(source!, target!, this.trigger!);
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

    private void TransitionIsNull()
    {
        this.instanceUnderTests.Should()
            .BeNull();
    }

    private void TransitionIsNotNull()
    {
        this.instanceUnderTests.Should()
            .NotBeNull();
    }

    private void SourcePropertyIs(string value)
    {
        this.instanceUnderTests!.Source.Name.Should().Be(value);
    }

    private void TargetPropertyIs(string value)
    {
        this.instanceUnderTests!.Target.Name.Should().Be(value);
    }

    private void TriggerPropertyIs(string value)
    {
        this.instanceUnderTests!.Trigger.Should().Be(value);
    }
}
