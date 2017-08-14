using System;

namespace SimpleSoft.Mediator.Example.Cmd
{
    public class ChangeUserPasswordCommand : Command
    {
        public ChangeUserPasswordCommand(Guid id, Guid userId, string oldPassword, string newPassword)
        {
            Id = id;
            UserId = userId;
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }

        public Guid UserId { get; }

        public string OldPassword { get; }

        public string NewPassword { get; }
    }
}