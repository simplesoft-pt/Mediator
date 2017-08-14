using System;

namespace SimpleSoft.Mediator.Example.Cmd.Middleware
{
    public class CommandFailedEvent : Event
    {
        public CommandFailedEvent(Guid commandId, string message)
        {
            CommandId = commandId;
            Message = message;
        }

        public Guid CommandId { get; }

        public string Message { get; }
    }
}