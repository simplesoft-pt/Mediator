using FluentValidation;

namespace SimpleSoft.Mediator.Example.Cmd.Commands
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(e => e.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(e => e.Name)
                .NotEmpty();
        }
    }
}