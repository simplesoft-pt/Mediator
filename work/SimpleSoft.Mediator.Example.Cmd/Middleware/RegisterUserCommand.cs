using System;

namespace SimpleSoft.Mediator.Example.Cmd.Middleware
{
    public class RegisterUserCommand : Command
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
