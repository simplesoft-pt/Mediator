using System;

namespace SimpleSoft.Mediator.Example.Cmd
{
    public class UserByIdQuery : Query<User>
    {
        public UserByIdQuery(Guid id, Guid userId)
        {
            Id = id;
            UserId = userId;
        }

        public Guid UserId { get; }
    }
}