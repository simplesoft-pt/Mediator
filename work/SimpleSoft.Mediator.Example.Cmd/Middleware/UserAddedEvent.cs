using System;

namespace SimpleSoft.Mediator.Example.Cmd.Middleware
{
    public class UserAddedEvent : Event
    {
        public UserAddedEvent(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; }
    }
}