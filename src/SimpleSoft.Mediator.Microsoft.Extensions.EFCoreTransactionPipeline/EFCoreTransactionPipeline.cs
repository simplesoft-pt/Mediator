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
            if (!_options.BeginTransactionOnCommand)
            {
                await next(cmd, ct);
                return;
            }

            _logger.LogDebug("Starting a command transaction");

#if NETSTANDARD2_1
            await using var tx = await _context.Database.BeginTransactionAsync(ct);
#else
            using var tx = await _context.Database.BeginTransactionAsync(ct);
#endif

            await next(cmd, ct);

            _logger.LogDebug("Saving changes into the database");

            await _context.SaveChangesAsync(ct);

#if NETSTANDARD2_1
            await tx.CommitAsync(ct);
#else
            tx.Commit();
#endif

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

#if NETSTANDARD2_1
            await using var tx = await _context.Database.BeginTransactionAsync(ct);
#else
            using var tx = await _context.Database.BeginTransactionAsync(ct);
#endif

            var result = await next(cmd, ct);

            _logger.LogDebug("Saving changes into the database");

            await _context.SaveChangesAsync(ct);

#if NETSTANDARD2_1
            await tx.CommitAsync(ct);
#else
            tx.Commit();
#endif

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

#if NETSTANDARD2_1
            await using var tx = await _context.Database.BeginTransactionAsync(ct);
#else
            using var tx = await _context.Database.BeginTransactionAsync(ct);
#endif

            await next(evt, ct);

            _logger.LogDebug("Saving changes into the database");

            await _context.SaveChangesAsync(ct);

#if NETSTANDARD2_1
            await tx.CommitAsync(ct);
#else
            tx.Commit();
#endif

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

#if NETSTANDARD2_1
            await using var tx = await _context.Database.BeginTransactionAsync(ct);
#else
            using var tx = await _context.Database.BeginTransactionAsync(ct);
#endif

            var result = await next(query, ct);

            _logger.LogDebug("Saving changes into the database");

            if (_options.ForceRollbackOnQuery)
            {

#if NETSTANDARD2_1
                await tx.RollbackAsync(ct);
#else
                tx.Rollback();
#endif

                _logger.LogInformation("Changes were reverted from the database");
            }
            else
            {
                await _context.SaveChangesAsync(ct);

#if NETSTANDARD2_1
                await tx.CommitAsync(ct);
#else
                tx.Commit();
#endif

                _logger.LogInformation("Changes committed into the database");
            }

            return result;
        }
    }
}
