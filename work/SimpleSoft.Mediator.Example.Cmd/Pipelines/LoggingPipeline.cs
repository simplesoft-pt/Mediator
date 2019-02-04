using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace SimpleSoft.Mediator.Example.Cmd.Pipelines
{
    public class LoggingPipeline : IPipeline
    {
        private readonly ILogger<LoggingPipeline> _logger;

        public LoggingPipeline(ILogger<LoggingPipeline> logger)
        {
            _logger = logger;
        }

        public Task OnCommandAsync<TCommand>(Func<TCommand, CancellationToken, Task> next, TCommand cmd, CancellationToken ct) where TCommand : class, ICommand
        {
            if (_logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("Command['{commandName}']:{command}", cmd.GetType().Name, SerializeToJson(cmd));

            return next(cmd, ct);
        }

        public async Task<TResult> OnCommandAsync<TCommand, TResult>(Func<TCommand, CancellationToken, Task<TResult>> next, TCommand cmd, CancellationToken ct) where TCommand : class, ICommand<TResult>
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                var commandName = cmd.GetType().Name;
                _logger.LogDebug("Command['{commandName}']:{command}", commandName, SerializeToJson(cmd));

                var result = await next(cmd, ct);

                _logger.LogDebug(
                    "Command['{commandName}'].Result:{commandResult}", commandName, SerializeToJson(result));
                return result;
            }

            return await next(cmd, ct);
        }

        public Task OnEventAsync<TEvent>(Func<TEvent, CancellationToken, Task> next, TEvent evt, CancellationToken ct) where TEvent : class, IEvent
        {
            if (_logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("Event['{eventName}']:{event}", evt.GetType().Name, SerializeToJson(evt));

            return next(evt, ct);
        }

        public Task<TResult> OnQueryAsync<TQuery, TResult>(Func<TQuery, CancellationToken, Task<TResult>> next, TQuery query, CancellationToken ct) where TQuery : class, IQuery<TResult>
        {
            if (_logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("Query['{queryName}']:{query}", query.GetType().Name, SerializeToJson(query));

            return next(query, ct);
        }

        private static string SerializeToJson(object value) => JsonConvert.SerializeObject(value, Formatting.Indented);
    }
}