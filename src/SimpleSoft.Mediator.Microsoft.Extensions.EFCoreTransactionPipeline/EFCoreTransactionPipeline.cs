using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// Entity Framework Core pipeline 
    /// </summary>
    public class EFCoreTransactionPipeline<TDbContext> : IPipeline where TDbContext : DbContext
    {
        private readonly TDbContext _context;
        private readonly EFCoreTransactionPipelineOptions _options;
        private readonly ILogger<EFCoreTransactionPipeline<TDbContext>> _logger;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="context"></param>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public EFCoreTransactionPipeline(
            TDbContext context,
            IOptions<EFCoreTransactionPipelineOptions> options,
            ILogger<EFCoreTransactionPipeline<TDbContext>> logger)
        {
            _context = context;
            _options = options.Value;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task OnCommandAsync<TCommand>(Func<TCommand, CancellationToken, Task> next, TCommand cmd, CancellationToken ct) where TCommand : class, ICommand
        {
            if (_options.BeginTransactionOnCommand)
            {
                _logger.LogDebug("Starting a command transaction");

                using (var tx = await _context.Database.BeginTransactionAsync(ct))
                {
                    await next(cmd, ct);

                    _logger.LogDebug("Saving changes into the database");

                    await _context.SaveChangesAsync(ct);
                    tx.Commit();

                    _logger.LogInformation("Changes committed into the database");
                }
            }
            else
                await next(cmd, ct);
        }

        /// <inheritdoc />
        public async Task<TResult> OnCommandAsync<TCommand, TResult>(Func<TCommand, CancellationToken, Task<TResult>> next, TCommand cmd, CancellationToken ct) where TCommand : class, ICommand<TResult>
        {
            TResult result;
            if (_options.BeginTransactionOnCommand)
            {
                _logger.LogDebug("Starting a command transaction");

                using (var tx = await _context.Database.BeginTransactionAsync(ct))
                {
                    result = await next(cmd, ct);

                    _logger.LogDebug("Saving changes into the database");

                    await _context.SaveChangesAsync(ct);
                    tx.Commit();

                    _logger.LogInformation("Changes committed into the database");
                }
            }
            else
                result = await next(cmd, ct);

            return result;
        }

        /// <inheritdoc />
        public async Task OnEventAsync<TEvent>(Func<TEvent, CancellationToken, Task> next, TEvent evt, CancellationToken ct) where TEvent : class, IEvent
        {
            if (_options.BeginTransactionOnEvent)
            {
                _logger.LogDebug("Starting an event transaction");

                using (var tx = await _context.Database.BeginTransactionAsync(ct))
                {
                    await next(evt, ct);

                    _logger.LogDebug("Saving changes into the database");

                    await _context.SaveChangesAsync(ct);
                    tx.Commit();

                    _logger.LogInformation("Changes committed into the database");
                }
            }
            else
                await next(evt, ct);
        }

        /// <inheritdoc />
        public async Task<TResult> OnQueryAsync<TQuery, TResult>(Func<TQuery, CancellationToken, Task<TResult>> next, TQuery query, CancellationToken ct) where TQuery : class, IQuery<TResult>
        {
            TResult result;
            if (_options.BeginTransactionOnQuery)
            {
                _logger.LogDebug("Starting a query transaction");

                using (var tx = await _context.Database.BeginTransactionAsync(ct))
                {
                    result = await next(query, ct);

                    _logger.LogDebug("Saving changes into the database");

                    await _context.SaveChangesAsync(ct);
                    tx.Commit();

                    _logger.LogInformation("Changes committed into the database");
                }
            }
            else
                result = await next(query, ct);

            return result;
        }
    }
}
