using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SimpleSoft.Mediator.Pipeline;

namespace SimpleSoft.Mediator.Example.Cmd
{
    public class IgnoreHandlerNotFoundExceptionMiddleware : Middleware
    {
        private readonly ILogger<IgnoreHandlerNotFoundExceptionMiddleware> _logger;

        public IgnoreHandlerNotFoundExceptionMiddleware(ILogger<IgnoreHandlerNotFoundExceptionMiddleware> logger)
        {
            _logger = logger;
        }

        public override async Task OnCommandAsync<TCommand>(CommandMiddlewareDelegate<TCommand> next, TCommand cmd, CancellationToken ct)
        {
            try
            {
                await next(cmd, ct).ConfigureAwait(false);
            }
            catch (CommandHandlerNotFoundException e)
            {
                _logger.LogWarning(0, e, "No handler was found for the command. Ignoring exception...");
            }
        }

        public override async Task<TResult> OnCommandAsync<TCommand, TResult>(CommandMiddlewareDelegate<TCommand, TResult> next, TCommand cmd, CancellationToken ct)
        {
            try
            {
                return await next(cmd, ct).ConfigureAwait(false);
            }
            catch (CommandHandlerNotFoundException e)
            {
                _logger.LogWarning(0, e, "No handler was found for the command. Ignoring exception...");
                return default(TResult);
            }
        }

        public override async Task<TResult> OnQueryAsync<TQuery, TResult>(QueryMiddlewareDelegate<TQuery, TResult> next, TQuery query, CancellationToken ct)
        {
            try
            {
                return await next(query, ct).ConfigureAwait(false);
            }
            catch (QueryHandlerNotFoundException e)
            {
                _logger.LogWarning(0, e, "No handler was found for the query. Ignoring exception...");
                return default(TResult);
            }
        }
    }
}
