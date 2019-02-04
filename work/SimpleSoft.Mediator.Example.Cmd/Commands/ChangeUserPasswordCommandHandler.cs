using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleSoft.Mediator.Example.Cmd.Commands
{
    public class ChangeUserPasswordCommandHandler : ICommandHandler<ChangeUserPasswordCommand>
    {
        private readonly IDictionary<Guid, User> _store;

        public ChangeUserPasswordCommandHandler(IDictionary<Guid, User> store)
        {
            _store = store;
        }

        public async Task HandleAsync(ChangeUserPasswordCommand cmd, CancellationToken ct)
        {
            User user;
            if (_store.TryGetValue(cmd.UserId, out user))
            {
                if (user.Password.Equals(cmd.OldPassword))
                {
                    await Task.Delay(1000, ct);

                    user.Password = cmd.NewPassword;
                    return;
                }

                throw new InvalidOperationException($"Invalid password for user id '{cmd.UserId}'");
            }

            throw new InvalidOperationException($"User id '{cmd.UserId}' could not be found");
        }
    }
}