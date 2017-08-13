using System;

namespace SimpleSoft.Mediator.Tests.Mocks
{
    public class MockEvent : Event
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