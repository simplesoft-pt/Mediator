using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SimpleSoft.Mediator.Example.Cmd.Events;

namespace SimpleSoft.Mediator.Example.Cmd.Commands
{
    public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Guid>
    {
        private readonly ConcurrentDictionary<string, User> _store;
        private readonly IMediator _mediator;
        private readonly ILogger<CreateUserCommandHandler> _logger;

        public CreateUserCommandHandler(ConcurrentDictionary<string, User> store, IMediator mediator,
            ILogger<CreateUserCommandHandler> logger)
        {
            _store = store;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Guid> HandleAsync(CreateUserCommand cmd, CancellationToken ct)
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

        public class Validator : AbstractValidator<CreateUserCommand>
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