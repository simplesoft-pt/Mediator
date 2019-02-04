using System;

namespace SimpleSoft.Mediator.Example.Cmd.Commands
{
    public class CreateUserCommand : Command<Guid>
    {
        public CreateUserCommand(string email, string name)
        {
            Email = email;
            Name = name;
        }

        public string Email { get; }

        public string Name { get; }
    }
}
