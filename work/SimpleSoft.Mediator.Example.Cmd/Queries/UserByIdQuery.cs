using System;

namespace SimpleSoft.Mediator.Example.Cmd.Queries
{
    public class UserByIdQuery : Query<User>
    {
        public UserByIdQuery(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; }
    }
}