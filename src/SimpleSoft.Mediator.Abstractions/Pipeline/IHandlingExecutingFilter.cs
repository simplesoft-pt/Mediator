using System.Threading;
using System.Threading.Tasks;

namespace SimpleSoft.Mediator.Pipeline
{
    /// <summary>
    /// Filter run before the handling of commands or events.
    /// </summary>
    public interface IHandlingExecutingFilter
    {
        /// <summary>
        /// Executed before the handling of an <see cref="ICommand"/>.
        /// </summary>
        /// <typeparam name="TCommand">The command type</typeparam>
        /// <param name="cmd">The command to be handled</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        Task OnExecutingCommandAsync<TCommand>(TCommand cmd, CancellationToken ct)
            where TCommand : ICommand;

        /// <summary>
        /// Executed before the handling of an <see cref="ICommand{TResult}"/>.
        /// </summary>
        /// <typeparam name="TCommand">The command type</typeparam>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="cmd">The command to be handled</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        Task OnExecutingCommandAsync<TCommand, TResult>(TCommand cmd, CancellationToken ct)
            where TCommand : ICommand<TResult>;

        /// <summary>
        /// Executed before the handling of an <see cref="IEvent"/>.
        /// </summary>
        /// <typeparam name="TEvent">The event type</typeparam>
        /// <param name="evt">The event to be handled</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        Task OnExecutingEventAsync<TEvent>(TEvent evt, CancellationToken ct)
            where TEvent : IEvent;
    }
}
