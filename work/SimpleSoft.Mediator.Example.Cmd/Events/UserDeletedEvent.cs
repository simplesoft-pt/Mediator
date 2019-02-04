using System;

namespace SimpleSoft.Mediator.Example.Cmd.Events
{
    public class UserDeletedEvent : Event
    {
        public UserDeletedEvent(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; }
    }
}
