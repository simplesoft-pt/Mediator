using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SimpleSoft.Mediator.Example.Cmd.Events;

namespace SimpleSoft.Mediator.Example.Cmd.Commands
{
    public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, Guid>
    {
        private readonly ConcurrentDictionary<string, User> _store;
        private readonly IMediator _mediator;
        private readonly ILogger<RegisterUserCommandHandler> _logger;

        public RegisterUserCommandHandler(ConcurrentDictionary<string, User> store, IMediator mediator,
            ILogger<RegisterUserCommandHandler> logger)
        {
            _store = store;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Guid> HandleAsync(RegisterUserCommand cmd, CancellationToken ct)
        {
            var email = cmd.Email.Trim().ToLowerInvariant();

            using (_logger.BeginScope("Email:'{email}'", email))
            {
                _logger.LogDebug("Creating user");

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Email = email,
                    Name = cmd.Name
                };

                if (_store.TryAdd(email, user))
                {
                    await _mediator.BroadcastAsync(new UserCreatedEvent(user.Id), ct);
                    return user.Id;
                }

                throw new InvalidOperationException($"Duplicated email '{email}'");
            }
        }

        public class Validator : AbstractValidator<RegisterUserCommand>
        {
            public Validator()
            {
                RuleFor(e => e.Email)
                    .NotEmpty()
                    .EmailAddress();

                RuleFor(e => e.Name)
                    .NotEmpty();
            }
        }
    }
}