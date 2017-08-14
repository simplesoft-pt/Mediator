using System;

namespace SimpleSoft.Mediator.Example.Cmd.Middleware
{
    public class UserUpdatedEvent : Event
    {
        public UserUpdatedEvent(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; }
    }
}