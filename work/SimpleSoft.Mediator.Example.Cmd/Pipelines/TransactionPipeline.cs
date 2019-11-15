using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SimpleSoft.Mediator.Example.Cmd.Pipelines
{
    public class TransactionPipeline : Pipeline
    {
        private readonly ILogger<TransactionPipeline> _logger;
        private readonly DbConnection _connection;

        public TransactionPipeline(ILogger<TransactionPipeline> logger)
        {
            _logger = logger;
            _connection = null; // DbContext, ISession... injected from container
        }

        public override async Task OnCommandAsync<TCommand>(Func<TCommand, CancellationToken, Task> next, TCommand cmd, CancellationToken ct)
        {
            _logger.LogDebug("Starting database transaction");

            using var tx = _connection?.BeginTransaction();

            await next(cmd, ct);

            _logger.LogDebug("Committing database transaction");
            tx?.Commit();
        }

        public override async Task<TResult> OnCommandAsync<TCommand, TResult>(Func<TCommand, CancellationToken, Task<TResult>> next, TCommand cmd, CancellationToken ct)
        {
            _logger.LogDebug("Starting database transaction");

            using var tx = _connection?.BeginTransaction();

            var result = await next(cmd, ct);

            _logger.LogDebug("Committing database transaction");
            tx?.Commit();

            return result;
        }
    }
}