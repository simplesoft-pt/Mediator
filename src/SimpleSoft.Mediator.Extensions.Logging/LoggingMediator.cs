using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// Mediator with logging support
    /// </summary>
    public class LoggingMediator : IMediator
    {
        private readonly ILogger<Mediator> _logger;
        private readonly IMediator _mediator;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="factory">The factory to be used</param>
        /// <param name="logger">The logger instance</param>
        /// <exception cref="ArgumentNullException"></exception>
        public LoggingMediator(IMediatorFactory factory, ILogger<Mediator> logger)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            if (logger == null) throw new ArgumentNullException(nameof(logger));

            _logger = logger;
            _mediator = new Mediator(factory);
        }

        /// <inheritdoc />
        public async Task PublishAsync<TCommand>(TCommand cmd, CancellationToken ct = new CancellationToken()) where TCommand : ICommand
        {
            if (cmd == null) throw new ArgumentNullException(nameof(cmd));

            using (_logger.BeginScope(
                "CommandName:{commandName} CommandId:{commandId}", typeof(TCommand).Name, cmd.Id))
            {
                _logger.LogDebug("Publishing command");
                await _mediator.PublishAsync(cmd, ct).ConfigureAwait(false);
            }
        }

        /// <inheritdoc />
        public async Task<TResult> PublishAsync<TCommand, TResult>(TCommand cmd, CancellationToken ct = new CancellationToken()) where TCommand : ICommand<TResult>
        {
            using (_logger.BeginScope(
                "CommandName:{commandName} CommandId:{commandId}", typeof(TCommand).Name, cmd.Id))
            {
                _logger.LogDebug("Publishing command");
                return await _mediator.PublishAsync<TCommand, TResult>(cmd, ct).ConfigureAwait(false);
            }
        }

        /// <inheritdoc />
        public async Task BroadcastAsync<TEvent>(TEvent evt, CancellationToken ct = new CancellationToken()) where TEvent : IEvent
        {
            if (evt == null) throw new ArgumentNullException(nameof(evt));

            using (_logger.BeginScope(
                "EventName:{eventName} EventId:{eventId}", typeof(TEvent).Name, evt.Id))
            {
                _logger.LogDebug("Broadcasting event");
                await _mediator.BroadcastAsync(evt, ct).ConfigureAwait(false);
            }
        }

        /// <inheritdoc />
        public async Task<TResult> FetchAsync<TQuery, TResult>(TQuery query, CancellationToken ct = new CancellationToken()) where TQuery : IQuery<TResult>
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            using (_logger.BeginScope(
                "QueryName:{queryName} QueryId:{queryId}", typeof(TQuery).Name, query.Id))
            {
                _logger.LogDebug("Fetching query data");
                return await _mediator.FetchAsync<TQuery, TResult>(query, ct).ConfigureAwait(false);
            }
        }
    }
}
