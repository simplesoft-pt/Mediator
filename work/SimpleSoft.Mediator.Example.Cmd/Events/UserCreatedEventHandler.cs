using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SimpleSoft.Mediator.Example.Cmd.Events
{
    public class UserCreatedEventHandler : IEventHandler<UserCreatedEvent>
    {
        private readonly ILogger<UserCreatedEvent> _logger;

        public UserCreatedEventHandler(ILogger<UserCreatedEvent> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(UserCreatedEvent evt, CancellationToken ct)
        {
            _logger.LogDebug("User '{userId}' was created", evt.UserId);

            return Task.CompletedTask;
        }
    }
}