using System;

namespace SimpleSoft.Mediator.Tests.Mocks
{
    public class MockQuery : Query<object>
    {
        public MockQuery()
        {

        }

        public MockQuery(Guid id, DateTimeOffset createdOn, string createdBy)
            : base(id, createdOn, createdBy)
        {

        }
    }
}
