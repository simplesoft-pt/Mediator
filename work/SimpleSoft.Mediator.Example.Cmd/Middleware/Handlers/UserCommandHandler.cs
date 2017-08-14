using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleSoft.Mediator.Example.Cmd.Middleware.Handlers
{
    public class UserCommandHandler : 
        ICommandHandler<RegisterUserCommand>,
        ICommandHandler<ChangeUserPasswordCommand>
    {
        private readonly IMediator _mediator;
        private readonly IDictionary<Guid, User> _users;

        public UserCommandHandler(IMediator mediator, IDictionary<Guid, User> users)
        {
            _mediator = mediator;
            _users = users;
        }

        public async Task HandleAsync(RegisterUserCommand cmd, CancellationToken ct)
        {
            if (_users.Values.Any(e => e.Email.Equals(cmd.Email, StringComparison.OrdinalIgnoreCase)))
            {
                await _mediator.BroadcastAsync(
                    new CommandFailedEvent(cmd.Id, "Duplicated email address"), ct);
            }
            else
            {
                var userId = Guid.NewGuid();
                _users.Add(userId, new User
                {
                    Email = cmd.Email,
                    Password = cmd.Password
                });

                await _mediator.BroadcastAsync(new UserAddedEvent(userId), ct);
            }
        }

        public async Task HandleAsync(ChangeUserPasswordCommand cmd, CancellationToken ct)
        {
            User user;
            if (_users.TryGetValue(cmd.UserId, out user))
            {
                if (user.Password.Equals(cmd.OldPassword))
                {
                    user.Password = cmd.NewPassword;
                    await _mediator.BroadcastAsync(new UserUpdatedEvent(cmd.UserId), ct);
                }
                else
                {
                    await _mediator.BroadcastAsync(
                        new CommandFailedEvent(cmd.Id, $"Invalid password for user '{cmd.UserId}'"), ct);
                }
            }
            else
            {
                await _mediator.BroadcastAsync(
                    new CommandFailedEvent(cmd.Id, $"User with id '{cmd.UserId}' not found"), ct);
            }
        }
    }
}