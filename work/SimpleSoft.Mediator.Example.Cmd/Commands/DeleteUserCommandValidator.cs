using FluentValidation;

namespace SimpleSoft.Mediator.Example.Cmd.Commands
{
    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator()
        {
            RuleFor(e => e.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}