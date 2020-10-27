using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// Logging pipeline
    /// </summary>
    public class LoggingPipeline : IPipeline
    {
        private readonly LoggingPipelineOptions _options;
        private readonly ILogger<LoggingPipeline> _logger;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public LoggingPipeline(
            IOptions<LoggingPipelineOptions> options,
            ILogger<LoggingPipeline> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        /// <inheritdoc />
        public Task OnCommandAsync<TCommand>(Func<TCommand, CancellationToken, Task> next, TCommand cmd, CancellationToken ct) 
            where TCommand : class, ICommand
        {
            var name = typeof(TCommand).Name;
            Log(_options.LogCommand, "Command['{commandName}']:{command}", name, cmd);

            return next(cmd, ct);
        }

        /// <inheritdoc />
        public async Task<TResult> OnCommandAsync<TCommand, TResult>(Func<TCommand, CancellationToken, Task<TResult>> next, TCommand cmd, CancellationToken ct) 
            where TCommand : class, ICommand<TResult>
        {
            var name = typeof(TCommand).Name;
            Log(_options.LogCommand, "Command['{commandName}']:{command}", name, cmd);

            var result = await next(cmd, ct).ConfigureAwait(false);

            Log(_options.LogCommandResult, "Command['{commandName}'].Result:{commandResult}", name, result);

            return result;
        }

        /// <inheritdoc />
        public Task OnEventAsync<TEvent>(Func<TEvent, CancellationToken, Task> next, TEvent evt, CancellationToken ct) 
            where TEvent : class, IEvent
        {
            var name = typeof(TEvent).Name;
            Log(_options.LogEvent, "Event['{eventName}']:{event}", name, evt);

            return next(evt, ct);
        }

        /// <inheritdoc />
        public async Task<TResult> OnQueryAsync<TQuery, TResult>(Func<TQuery, CancellationToken, Task<TResult>> next, TQuery query, CancellationToken ct) 
            where TQuery : class, IQuery<TResult>
        {
            var name = typeof(TQuery).Name;
            Log(_options.LogQuery, "Query['{queryName}']:{query}", name, query);

            var result = await next(query, ct).ConfigureAwait(false);

            Log(_options.LogQueryResult, "Query['{queryName}'].Result:{queryResult}", name, result);

            return result;
        }

        private void Log<T>(bool isActive, string message, string name, T instance)
        {
            if (!_logger.IsEnabled(_options.Level) || !isActive) 
                return;

            var serializedInstance = _options.Serializer(instance);
            switch (_options.Level)
            {
                case LogLevel.Trace:
                    _logger.LogTrace(message, name, serializedInstance);
                    break;
                case LogLevel.Debug:
                    _logger.LogDebug(message, name, serializedInstance);
                    break;
                case LogLevel.Information:
                    _logger.LogInformation(message, name, serializedInstance);
                    break;
                case LogLevel.Warning:
                    _logger.LogWarning(message, name, serializedInstance);
                    break;
                case LogLevel.Error:
                    _logger.LogError(message, name, serializedInstance);
                    break;
                case LogLevel.Critical:
                    _logger.LogCritical(message, name, serializedInstance);
                    break;
            }
        }
    }
}
