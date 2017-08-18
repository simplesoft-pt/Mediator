using System;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleSoft.Mediator.Pipeline
{
    /// <summary>
    /// Filter executed when the handling of commands or events fails.
    /// </summary>
    public interface IHandlingFailedFilter
    {
        /// <summary>
        /// Executed when the handling of an <see cref="ICommand"/> fails.
        /// </summary>
        /// <typeparam name="TCommand">The command type</typeparam>
        /// <param name="cmd">The command to be handled</param>
        /// <param name="exception">The exception thrown</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        Task OnFailedCommandAsync<TCommand>(TCommand cmd, Exception exception, CancellationToken ct)
            where TCommand : ICommand;

        /// <summary>
        /// Executed when the handling of an <see cref="ICommand{TResult}"/> fails.
        /// </summary>
        /// <typeparam name="TCommand">The command type</typeparam>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="cmd">The command to be handled</param>
        /// <param name="exception">The exception thrown</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        Task OnFailedCommandAsync<TCommand, TResult>(TCommand cmd, Exception exception, CancellationToken ct)
            where TCommand : ICommand<TResult>;

        /// <summary>
        /// Executed when the handling of an <see cref="IEvent"/> fails.
        /// </summary>
        /// <typeparam name="TEvent">The event type</typeparam>
        /// <param name="evt">The event to be handled</param>
        /// <param name="exception">The exception thrown</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        Task OnFailedEventAsync<TEvent>(TEvent evt, Exception exception, CancellationToken ct)
            where TEvent : IEvent;
    }
}