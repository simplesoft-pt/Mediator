using System;

namespace SimpleSoft.Mediator.Example.Cmd.Events
{
    public class UserCreatedEvent : Event
    {
        public UserCreatedEvent(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; }
    }
}
