using System;
using SimpleSoft.Mediator.Tests.Mocks;
using Xunit;

namespace SimpleSoft.Mediator.Tests
{
    public class CommandHandlerNotFoundExceptionTests
    {
        [Fact]
        public void GivenANullCommandWhenBuildingACommandHandlerNotFoundExceptionThenArgumentNullExceptionMustBeThrown()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                var e = CommandHandlerNotFoundException.Build<ICommand>(null);
                Assert.Null(e);
            });

            Assert.NotNull(ex);
        }

        [Fact]
        public void GivenANullCommandWithResultWhenBuildingACommandHandlerNotFoundExceptionThenArgumentNullExceptionMustBeThrown()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                var e = CommandHandlerNotFoundException.Build<ICommand<object>, object>(null);
                Assert.Null(e);
            });

            Assert.NotNull(ex);
        }

        [Fact]
        public void GivenACommandWhenBuildingACommandHandlerNotFoundExceptionThenAllPropertiesMustBeCorrect()
        {
            var cmd = new MockCommand();
            var commandType = cmd.GetType();

            var ex = CommandHandlerNotFoundException.Build(cmd);

            Assert.NotNull(ex.CommandData);
            Assert.Same(cmd, ex.CommandData);

            Assert.NotNull(ex.CommandType);
            Assert.Same(commandType, ex.CommandType);

            Assert.NotNull(ex.CommandName);
            Assert.Equal(commandType.Name, ex.CommandName);

            Assert.NotNull(ex.Message);
            Assert.Contains(ex.CommandName, ex.Message);

            Assert.NotNull(ex.Command<MockCommand>());
            Assert.Throws<InvalidCastException>(() =>
            {
                ex.Command<ICommand<object>, object>();
            });
        }

        [Fact]
        public void GivenACommandWithResultWhenBuildingACommandHandlerNotFoundExceptionThenAllPropertiesMustBeCorrect()
        {
            var cmd = new MockResultCommand();
            var commandType = cmd.GetType();

            var ex = CommandHandlerNotFoundException.Build<MockResultCommand, object>(cmd);

            Assert.NotNull(ex.CommandData);
            Assert.Same(cmd, ex.CommandData);

            Assert.NotNull(ex.CommandType);
            Assert.Same(commandType, ex.CommandType);

            Assert.NotNull(ex.CommandName);
            Assert.Equal(commandType.Name, ex.CommandName);

            Assert.NotNull(ex.Message);
            Assert.Contains(ex.CommandName, ex.Message);

            Assert.NotNull(ex.Command<MockResultCommand, object>());
            Assert.Throws<InvalidCastException>(() =>
            {
                ex.Command<ICommand>();
            });
        }
    }
}
