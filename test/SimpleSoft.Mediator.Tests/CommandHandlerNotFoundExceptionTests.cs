using System;
using SimpleSoft.Mediator.Tests.Mocks;
using Xunit;

namespace SimpleSoft.Mediator.Tests
{
    public class CommandHandlerNotFoundExceptionTests
    {
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
            Assert.True(ex.Message.Contains(ex.CommandName));
            
            Assert.NotNull(ex.Command<MockCommand>());
            Assert.Throws<InvalidCastException>(() =>
            {
                ex.Command<MockResultCommand, object>();
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
            Assert.True(ex.Message.Contains(ex.CommandName));

            Assert.NotNull(ex.Command<MockResultCommand, object>());
            Assert.Throws<InvalidCastException>(() =>
            {
                ex.Command<MockCommand>();
            });
        }
    }
}
