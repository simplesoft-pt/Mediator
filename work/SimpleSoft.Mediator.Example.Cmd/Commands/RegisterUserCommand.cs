using System;

namespace SimpleSoft.Mediator.Example.Cmd.Commands
{
    public class RegisterUserCommand : Command<Guid>
    {
        public RegisterUserCommand(Guid id, string email, string password)
        {
            Id = id;
            Email = email;
            Password = password;
        }

        public string Email { get; }

        public string Password { get; }
    }
}
