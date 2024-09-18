namespace MicroDotNet.Services.StateMachines.Domain.UnitTests.MachineStructureTests.StatusTests
{
    using FluentAssertions;

    using MicroDotNet.Services.StateMachines.Domain.MachineStructure;

    using TestStack.BDDfy;

    public sealed class ParseTests
    {
        private string code = default!;

        private Status parsed = default!;

        private Action parseAction = default!;

        [Fact]
        public void WhenCorrectCodeIsParsedThenItReturnsCorrectValue()
        {
            this.Given(t => t.CodeIs(Status.Completed.Code))
                .When(t => t.CodeIsParsed())
                .Then(t => t.ExceptionIsNotThrown())
                .And(t => t.ParsedValueIs(Status.Completed))
                .BDDfy();
        }

        [Fact]
        public void WhenEmptyCodeIsParsedThenCorrectExceptionIsThrown()
        {
            this.Given(t => t.CodeIs(string.Empty))
                .When(t => t.CodeIsParsed())
                .Then(t => t.ExceptionIsThrown<ArgumentException>(e => e.ParamName == "code"))
                .BDDfy();
        }

        [Fact]
        public void WhenNonExistingCodeIsParsedThenCorrectExceptionIsThrown()
        {
            this.Given(t => t.CodeIs("SOME_RANDOM_TEXT"))
                .When(t => t.CodeIsParsed())
                .Then(t => t.ExceptionIsThrown<InvalidOperationException>(e => e != null))
                .BDDfy();
        }

        private void CodeIs(string value)
        {
            this.code = value;
        }

        private void CodeIsParsed()
        {
            this.parseAction = () => this.parsed = Status.Parse(this.code);
        }

        private void ExceptionIsNotThrown()
        {
            this.parseAction.Should()
                .NotThrow();
        }

        private void ExceptionIsThrown<TException>(Func<TException, bool> exceptionPredicate)
            where TException : Exception
        {
            this.parseAction.Should().Throw<TException>()
                .And.Should().Match(e => exceptionPredicate((TException)e));
        }

        private void ParsedValueIs(Status value)
        {
            this.parsed.Should()
                .BeSameAs(value);
        }
    }
}
