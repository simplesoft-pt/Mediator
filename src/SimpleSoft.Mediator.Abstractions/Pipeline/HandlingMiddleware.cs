using System.Threading;
using System.Threading.Tasks;

namespace SimpleSoft.Mediator.Pipeline
{
    /// <summary>
    /// Handling middleware that can be used to intercept commands and events
    /// </summary>
    public abstract class HandlingMiddleware : IHandlingMiddleware
    {
        /// <inheritdoc />
        public virtual async Task OnCommandAsync<TCommand>(HandlingCommandDelegate<TCommand> next, TCommand cmd, CancellationToken ct) 
            where TCommand : ICommand
        {
            await next(cmd, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public virtual async Task<TResult> OnCommandAsync<TCommand, TResult>(HandlingCommandDelegate<TCommand, TResult> next, TCommand cmd, CancellationToken ct)
            where TCommand : ICommand<TResult>
        {
            return await next(cmd, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public virtual async Task OnEventAsync<TEvent>(HandlingEventDelegate<TEvent> next, TEvent evt, CancellationToken ct) 
            where TEvent : IEvent
        {
            await next(evt, ct).ConfigureAwait(false);
        }
    }
}