using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SimpleSoft.Mediator.Pipeline;

namespace SimpleSoft.Mediator.Example.Cmd
{
    public class LoggingMiddleware : Middleware
    {
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(ILogger<LoggingMiddleware> logger)
        {
            _logger = logger;
        }

        public override async Task OnCommandAsync<TCommand>(CommandMiddlewareDelegate<TCommand> next, TCommand cmd, CancellationToken ct)
        {
            try
            {
                _logger.LogDebug("Command about to be published");

                await next(cmd, ct).ConfigureAwait(false);

                _logger.LogDebug("Command was published");
            }
            catch (TaskCanceledException)
            {
                _logger.LogWarning("Command publish operation was canceled");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(0, e, "Command publish failed");
                throw;
            }
        }
        
        public override async Task<TResult> OnCommandAsync<TCommand, TResult>(CommandMiddlewareDelegate<TCommand, TResult> next, TCommand cmd, CancellationToken ct)
        {
            try
            {
                _logger.LogDebug("Command with result about to be published");

                var result = await next(cmd, ct).ConfigureAwait(false);

                _logger.LogDebug("Command with result was published");
                return result;
            }
            catch (TaskCanceledException)
            {
                _logger.LogWarning("Command with result publish operation was canceled");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(0, e, "Command with result publish failed");
                throw;
            }
        }
        
        public override async Task OnEventAsync<TEvent>(EventMiddlewareDelegate<TEvent> next, TEvent evt, CancellationToken ct)
        {
            try
            {
                _logger.LogDebug("Event about to be broadcast");

                await next(evt, ct).ConfigureAwait(false);

                _logger.LogDebug("Event was broadcast");
            }
            catch (TaskCanceledException)
            {
                _logger.LogWarning("Event broadcast operation was canceled");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(0, e, "Event broadcast failed");
                throw;
            }
        }

        public override async Task<TResult> OnQueryAsync<TQuery, TResult>(QueryMiddlewareDelegate<TQuery, TResult> next, TQuery query, CancellationToken ct)
        {
            try
            {
                _logger.LogDebug("Query about to be fetched");

                var result = await next(query, ct).ConfigureAwait(false);

                _logger.LogDebug("Query was fetched");
                return result;
            }
            catch (TaskCanceledException)
            {
                _logger.LogWarning("Query fetch operation was canceled");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(0, e, "Query fetch failed");
                throw;
            }
        }
    }
}