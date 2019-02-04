using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SimpleSoft.Mediator.Example.Cmd.Events
{
    public class UserDeletedEventHandler : IEventHandler<UserDeletedEvent>
    {
        private readonly ILogger<UserDeletedEventHandler> _logger;

        public UserDeletedEventHandler(ILogger<UserDeletedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(UserDeletedEvent evt, CancellationToken ct)
        {
            _logger.LogDebug("User '{userId}' was deleted", evt.UserId);

            return Task.CompletedTask;
        }
    }
}