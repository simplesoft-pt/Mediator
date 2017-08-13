using System;

namespace SimpleSoft.Mediator.Tests.Mocks
{
    public class MockResultCommand : Command<object>
    {
        public MockResultCommand()
        {

        }

        public MockResultCommand(Guid id, DateTimeOffset createdOn, string createdBy)
            : base(id, createdOn, createdBy)
        {

        }
    }
}