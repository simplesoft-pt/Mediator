using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SimpleSoft.Database;

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// SimpleSoft Database pipeline
    /// </summary>
    public class DatabaseTransactionPipeline : IPipeline
    {
        private readonly ITransaction _transaction;
        private readonly DatabaseTransactionPipelineOptions _options;
        private readonly ILogger<DatabaseTransactionPipeline> _logger;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public DatabaseTransactionPipeline(
            ITransaction transaction,
            IOptions<DatabaseTransactionPipelineOptions> options,
            ILogger<DatabaseTransactionPipeline> logger)
        {
            _transaction = transaction;
            _options = options.Value;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task OnCommandAsync<TCommand>(Func<TCommand, CancellationToken, Task> next, TCommand cmd, CancellationToken ct) where TCommand : class, ICommand
        {
            if (!_options.BeginTransactionOnCommand)
            {
                await next(cmd, ct);
                return;
            }

            _logger.LogDebug("Starting a command transaction");

            await _transaction.BeginAsync(ct);

            await next(cmd, ct);

            _logger.LogDebug("Saving changes into the database");

            await _transaction.CommitAsync(ct);

            _logger.LogInformation("Changes committed into the database");
        }

        /// <inheritdoc />
        public async Task<TResult> OnCommandAsync<TCommand, TResult>(Func<TCommand, CancellationToken, Task<TResult>> next, TCommand cmd, CancellationToken ct) where TCommand : class, ICommand<TResult>
        {
            if (!_options.BeginTransactionOnCommand)
            {
                return await next(cmd, ct);
            }
            
            _logger.LogDebug("Starting a command transaction");

            await _transaction.BeginAsync(ct);

            var result = await next(cmd, ct);

            _logger.LogDebug("Saving changes into the database");

            await _transaction.CommitAsync(ct);

            _logger.LogInformation("Changes committed into the database");

            return result;
        }

        /// <inheritdoc />
        public async Task OnEventAsync<TEvent>(Func<TEvent, CancellationToken, Task> next, TEvent evt, CancellationToken ct) where TEvent : class, IEvent
        {
            if (!_options.BeginTransactionOnEvent)
            {
                await next(evt, ct);
                return;
            }

            _logger.LogDebug("Starting an event transaction");

            await _transaction.BeginAsync(ct);

            await next(evt, ct);

            _logger.LogDebug("Saving changes into the database");

            await _transaction.CommitAsync(ct);

            _logger.LogInformation("Changes committed into the database");
        }

        /// <inheritdoc />
        public async Task<TResult> OnQueryAsync<TQuery, TResult>(Func<TQuery, CancellationToken, Task<TResult>> next, TQuery query, CancellationToken ct) where TQuery : class, IQuery<TResult>
        {
            if (!_options.BeginTransactionOnQuery)
            {
                return await next(query, ct);
            }

            _logger.LogDebug("Starting a query transaction");

            await _transaction.BeginAsync(ct);

            var result = await next(query, ct);

            _logger.LogDebug("Saving changes into the database");

            if (_options.ForceRollbackOnQuery)
            {
                await _transaction.RollbackAsync(ct);

                _logger.LogInformation("Changes were reverted from the database");
            }
            else
            {
                await _transaction.CommitAsync(ct);

                _logger.LogInformation("Changes committed into the database");
            }

            return result;
        }
    }
}
