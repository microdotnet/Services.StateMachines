namespace MicroDotNet.Services.StateMachines.Domain.UnitTests.MachineStructureTests.MachineDefinitionAggreagateRootTests
{
    using System.Linq.Expressions;

    using FluentAssertions;

    using MicroDotNet.Services.StateMachines.Domain.MachineStructure;
    using MicroDotNet.Services.StateMachines.Domain.MachineStructure.Events;

    using TestStack.BDDfy;

    public sealed class CreateTests
    {
        private MachineName machineName = default!;

        private MachineDefinitionAggregateRoot instance = default!;

        private Action creationAction = () => { };

        [Fact]
        public void WhenInitialDataIsCorrectThenCorrectMachineIsCreated()
        {
            this.Given(t => t.MachineNameIs(MachineName.Create("MACHINECODE", 1)))
                .When(t => t.MachineDefinitionIsCreated())
                .Then(t => t.ExceptionIsNotThrown())
                .And(t => t.MachineNamePropertyIs(this.machineName))
                .And(t => t.DequeuedEventsShouldContain(e => e.GetType() == typeof(MachineDefinitionCreated), "should contain machine creation event"))
                .BDDfy();
        }

        [Fact]
        public void WhenInitialDataIsCorrectThenMachineContainsCorrectEvent()
        {
            this.Given(t => t.MachineNameIs(MachineName.Create("MACHINECODE", 1)))
                .When(t => t.MachineDefinitionIsCreated())
                .Then(t => t.ExceptionIsNotThrown())
                .And(t => t.MachineNamePropertyIs(this.machineName))
                .And(t => t.DequeuedEventsShouldContain(e => ((MachineDefinitionCreated)e).MachineName == this.machineName, "event should contain correct machine name"))
                .BDDfy();
        }

        [Fact]
        public void WhenMachineNameIsNotPrivdedThenMachineIsNotCreated()
        {
            this.Given(t => t.MachineNameIsNull())
                .When(t => t.MachineDefinitionIsCreated())
                .Then(t => t.ExceptionIsThrown<ArgumentNullException>(e => e.ParamName == "machineName"))
                .BDDfy();
        }

        private void MachineNameIs(MachineName value)
        {
            this.machineName = value;
        }

        private void MachineNameIsNull()
        {
            this.machineName = null!;
        }

        private void MachineDefinitionIsCreated()
        {
            this.creationAction = () => this.instance = MachineDefinitionAggregateRoot.Create(this.machineName);
        }

        private void ExceptionIsNotThrown()
        {
            this.creationAction.Should().NotThrow();
        }

        private void ExceptionIsThrown<TException>(Expression<Func<TException, bool>> predicate)
            where TException : Exception
        {
            this.creationAction.Should()
                .Throw<TException>()
                .Which.Should().Match(predicate);
        }

        private void MachineNamePropertyIs(MachineName value)
        {
            this.instance.MachineName.Should().Be(value);
        }

        private void DequeuedEventsShouldContain(Expression<Func<object, bool>> finder, string message)
        {
            var events = this.instance.DequeueUncommittedEvents().ToList();
            events.Should()
                .Contain(finder, message);
        }
    }
}
