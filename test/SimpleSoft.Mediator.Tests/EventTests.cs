using System;
using Xunit;

namespace SimpleSoft.Mediator.Tests
{
    public class EventTests
    {
        [Fact]
        public void GivenAnEventWhenUsingDefaultConstructorThenDefaultValuesMustBeUsed()
        {
            var now = DateTimeOffset.Now;

            var evt = new MockEvent();

            Assert.NotEqual(default(Guid), evt.Id);
            Assert.NotEqual(default(DateTimeOffset), evt.CreatedOn);
            Assert.True(now <= evt.CreatedOn && evt.CreatedOn <= DateTimeOffset.Now);
            Assert.Null(evt.CreatedBy);
        }

        [Fact]
        public void GivenAnEventWhenUsingConstructorWithParametersThenAllValuesMustBeEqual()
        {
            var id = Guid.NewGuid();
            var createdOn = DateTimeOffset.Now.AddMinutes(-1);
            const string createdBy = "test-user";

            var evt = new MockEvent(id, createdOn, createdBy);

            Assert.Equal(id, evt.Id);
            Assert.Equal(createdOn, evt.CreatedOn);
            Assert.Equal(createdBy, evt.CreatedBy);
        }

        private class MockEvent : Event
        {
            public MockEvent()
            {
                
            }

            public MockEvent(Guid id, DateTimeOffset createdOn, string createdBy) 
                : base(id, createdOn, createdBy)
            {

            }
        }
    }
}
