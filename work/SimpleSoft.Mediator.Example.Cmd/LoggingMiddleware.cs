using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug(
                        "Command sent: {command}", JsonConvert.SerializeObject(cmd));
                }

                await next(cmd, ct).ConfigureAwait(false);

                _logger.LogDebug("Command handled: <no result>");
            }
            catch (TaskCanceledException)
            {
                _logger.LogWarning("Command handling was canceled");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(0, e, "Command handling failed");
                throw;
            }
        }
        
        public override async Task<TResult> OnCommandAsync<TCommand, TResult>(CommandMiddlewareDelegate<TCommand, TResult> next, TCommand cmd, CancellationToken ct)
        {
            try
            {
                if (_logger.IsEnabled(LogLevel.Debug))
                    _logger.LogDebug(
                        "Command sent: {command}", JsonConvert.SerializeObject(cmd));

                var result = await next(cmd, ct).ConfigureAwait(false);

                if (_logger.IsEnabled(LogLevel.Debug))
                    _logger.LogDebug(
                        "Command handled: {commandResult}", JsonConvert.SerializeObject(result));
                return result;
            }
            catch (TaskCanceledException)
            {
                _logger.LogWarning("Command handling was canceled");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(0, e, "Command handling failed");
                throw;
            }
        }
        
        public override async Task OnEventAsync<TEvent>(EventMiddlewareDelegate<TEvent> next, TEvent evt, CancellationToken ct)
        {
            try
            {
                if (_logger.IsEnabled(LogLevel.Debug))
                    _logger.LogDebug(
                        "Event broadcasted: {event}", JsonConvert.SerializeObject(evt));

                await next(evt, ct).ConfigureAwait(false);

                _logger.LogDebug("Event handled");
            }
            catch (TaskCanceledException)
            {
                _logger.LogWarning("Event handling was canceled");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(0, e, "Event handling failed");
                throw;
            }
        }

        public override async Task<TResult> OnQueryAsync<TQuery, TResult>(QueryMiddlewareDelegate<TQuery, TResult> next, TQuery query, CancellationToken ct)
        {
            try
            {
                if (_logger.IsEnabled(LogLevel.Debug))
                    _logger.LogDebug(
                        "Query fetched: {query}", JsonConvert.SerializeObject(query));

                var result = await next(query, ct).ConfigureAwait(false);


                if (_logger.IsEnabled(LogLevel.Debug))
                    _logger.LogDebug(
                        "Query handled: {queryResult}", JsonConvert.SerializeObject(result));
                return result;
            }
            catch (TaskCanceledException)
            {
                _logger.LogWarning("Query handling was canceled");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(0, e, "Query handling failed");
                throw;
            }
        }
    }
}