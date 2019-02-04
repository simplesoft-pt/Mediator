using System;

namespace SimpleSoft.Mediator.Example.Cmd.Commands
{
    public class RegisterUserCommand : Command<Guid>
    {
        public RegisterUserCommand(string email, string name)
        {
            Email = email;
            Name = name;
        }

        public string Email { get; }

        public string Name { get; }
    }
}
