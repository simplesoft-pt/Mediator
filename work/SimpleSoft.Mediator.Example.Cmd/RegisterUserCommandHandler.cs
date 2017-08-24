using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleSoft.Mediator.Example.Cmd
{
    public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, Guid>
    {
        private readonly IDictionary<Guid, User> _store;

        public RegisterUserCommandHandler(IDictionary<Guid, User> store)
        {
            _store = store;
        }

        public async Task<Guid> HandleAsync(RegisterUserCommand cmd, CancellationToken ct)
        {
            if (_store.Values.Any(e => e.Email.Equals(cmd.Email, StringComparison.OrdinalIgnoreCase)))
            {
                // this could be a fail event instead
                //  eg. await _mediator.BroadcastAsync(new CommandFailedEvent(cmd.Id), ct);
                throw new InvalidOperationException("Duplicated user email");
            }

            await Task.Delay(1000, ct);

            var userId = Guid.NewGuid();
            _store.Add(userId, new User
            {
                Email = cmd.Email,
                Password = cmd.Password
            });

            //  this could be a UserCreatedEvent instead of a command returning a result
            //  eg. await _mediator.BroadcastAsync(new UserCreatedEvent(userId), ct);
            return userId;
        }
    }
}