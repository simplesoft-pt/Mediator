using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SimpleSoft.Mediator.Example.Cmd.Events;

namespace SimpleSoft.Mediator.Example.Cmd.Commands
{
    public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand>
    {
        private readonly ConcurrentDictionary<string, User> _store;
        private readonly IMediator _mediator;
        private readonly ILogger<DeleteUserCommandHandler> _logger;

        public DeleteUserCommandHandler(
            ConcurrentDictionary<string, User> store,
            IMediator mediator,
            ILogger<DeleteUserCommandHandler> logger)
        {
            _store = store;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task HandleAsync(DeleteUserCommand cmd, CancellationToken ct)
        {
            var email = cmd.Email.Trim().ToLowerInvariant();

            using (_logger.BeginScope("Email:'{email}'", email))
            {
                _logger.LogDebug("Deleting user");

                if (_store.TryRemove(email, out var user))
                    await _mediator.BroadcastAsync(new UserDeletedEvent(user.Id), ct);
                else
                    throw new InvalidOperationException($"User with email '{email}' not found");
            }
        }
    }
}