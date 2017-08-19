using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SimpleSoft.Mediator.Example.Cmd
{
    public class Application
    {
        private readonly ILogger<Application> _logger;
        private readonly IMediator _mediator;

        public Application(ILogger<Application> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task RunAsync(CancellationToken ct)
        {
            for (var i = 0; i < 20; i++)
            {
                _logger.LogDebug("Creating new user");
                var userId = await _mediator.PublishAsync<RegisterUserCommand, Guid>(
                    new RegisterUserCommand(Guid.NewGuid(), $"someuser{i:D2}@domain.com", "123456"), ct);

                _logger.LogDebug("Getting user '{userId}'", userId);
                var user = await _mediator.FetchAsync<UserByIdQuery, User>(
                    new UserByIdQuery(Guid.NewGuid(), userId), ct);

                if (user == null)
                {
                    _logger.LogDebug("User '{userId}' could not be found");
                }
                else
                {
                    _logger.LogDebug("Changing password for user '{userId}'", userId);
                    await _mediator.PublishAsync(new ChangeUserPasswordCommand(
                        Guid.NewGuid(), userId, "123456", "654321"), ct);
                }
            }
        }
    }
}