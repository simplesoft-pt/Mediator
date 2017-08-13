using System;
using Xunit;

namespace SimpleSoft.Mediator.Tests
{
    public class CommandTests
    {
        [Fact]
        public void GivenACommandWhenUsingDefaultConstructorThenDefaultValuesMustBeUsed()
        {
            var now = DateTimeOffset.Now;

            var evt = new MockCommand();

            Assert.NotEqual(default(Guid), evt.Id);
            Assert.NotEqual(default(DateTimeOffset), evt.CreatedOn);
            Assert.True(now <= evt.CreatedOn && evt.CreatedOn <= DateTimeOffset.Now);
            Assert.Null(evt.CreatedBy);
        }

        [Fact]
        public void GivenACommandWithResultWhenUsingDefaultConstructorThenDefaultValuesMustBeUsed()
        {
            var now = DateTimeOffset.Now;

            var evt = new MockCommand<object>();

            Assert.NotEqual(default(Guid), evt.Id);
            Assert.NotEqual(default(DateTimeOffset), evt.CreatedOn);
            Assert.True(now <= evt.CreatedOn && evt.CreatedOn <= DateTimeOffset.Now);
            Assert.Null(evt.CreatedBy);
        }

        [Fact]
        public void GivenACommandWhenUsingConstructorWithParametersThenAllValuesMustBeEqual()
        {
            var id = Guid.NewGuid();
            var createdOn = DateTimeOffset.Now.AddMinutes(-1);
            const string createdBy = "test-user";

            var evt = new MockCommand(id, createdOn, createdBy);

            Assert.Equal(id, evt.Id);
            Assert.Equal(createdOn, evt.CreatedOn);
            Assert.Equal(createdBy, evt.CreatedBy);
        }

        [Fact]
        public void GivenACommandWithResultWhenUsingConstructorWithParametersThenAllValuesMustBeEqual()
        {
            var id = Guid.NewGuid();
            var createdOn = DateTimeOffset.Now.AddMinutes(-1);
            const string createdBy = "test-user";

            var evt = new MockCommand<object>(id, createdOn, createdBy);

            Assert.Equal(id, evt.Id);
            Assert.Equal(createdOn, evt.CreatedOn);
            Assert.Equal(createdBy, evt.CreatedBy);
        }

        private class MockCommand : Command
        {
            public MockCommand()
            {
                
            }

            public MockCommand(Guid id, DateTimeOffset createdOn, string createdBy)
                : base(id, createdOn, createdBy)
            {

            }
        }

        private class MockCommand<TResult> : Command<TResult>
        {
            public MockCommand()
            {

            }

            public MockCommand(Guid id, DateTimeOffset createdOn, string createdBy)
                : base(id, createdOn, createdBy)
            {

            }
        }
    }
}
