namespace SimpleSoft.Mediator.Example.Cmd.Commands
{
    public class DeleteUserCommand : Command
    {
        public DeleteUserCommand(string email)
        {
            Email = email;
        }

        public string Email { get; }
    }
}
