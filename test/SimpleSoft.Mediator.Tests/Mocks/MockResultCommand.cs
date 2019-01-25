using System;

namespace SimpleSoft.Mediator.Tests.Mocks
{
    public class MockResultCommand : Command<object>
    {
        public MockResultCommand()
        {

        }

        public MockResultCommand(Guid id, DateTimeOffset createdOn, string createdBy)
        {
            Id = id;
            CreatedOn = createdOn;
            CreatedBy = createdBy;
        }
    }
}