using System.Threading;
using System.Threading.Tasks;

namespace SimpleSoft.Mediator.Pipeline
{
    /// <summary>
    /// Handling middleware interface that can be used to intercept commands and events
    /// </summary>
    public interface IHandlingMiddleware
    {
        /// <summary>
        /// Method invoked when an <see cref="ICommand"/> is published.
        /// </summary>
        /// <typeparam name="TCommand">The command type</typeparam>
        /// <param name="next">The next middleware into the chain</param>
        /// <param name="cmd">The command published</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        Task OnCommandAsync<TCommand>(IHandlingMiddleware next, TCommand cmd, CancellationToken ct)
            where TCommand : ICommand;

        /// <summary>
        /// Method invoked when an <see cref="ICommand{TResult}"/> is published.
        /// </summary>
        /// <typeparam name="TCommand">The command type</typeparam>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="next">The next middleware into the chain</param>
        /// <param name="cmd">The command published</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited for the result</returns>
        Task<TResult> OnCommandAsync<TCommand, TResult>(IHandlingMiddleware next, TCommand cmd, CancellationToken ct)
            where TCommand : ICommand<TResult>;

        /// <summary>
        /// Method invoked when an <see cref="IEvent"/> is broadcast.
        /// </summary>
        /// <typeparam name="TEvent">The event type</typeparam>
        /// <param name="next">The next middleware into the chain</param>
        /// <param name="cmd">The event broadcasted</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        Task OnEventAsync<TEvent>(IHandlingMiddleware next, TEvent cmd, CancellationToken ct)
            where TEvent : IEvent;
    }
}
