using System.Threading;
using System.Threading.Tasks;

namespace SimpleSoft.Mediator.Pipeline
{
    /// <summary>
    /// Handler filter
    /// </summary>
    public interface IFilter
    {
        /// <summary>
        /// The filter order
        /// </summary>
        int Order { get; }
        
        /// <summary>
        /// Runs before the command handler
        /// </summary>
        /// <typeparam name="TCommand">The command type</typeparam>
        /// <param name="context">The command context</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        Task BeforeHandleAsync<TCommand>(
            FilterCommandContext<TCommand> context, CancellationToken ct = default(CancellationToken))
            where TCommand : ICommand;

        /// <summary>
        /// Runs before the command handler
        /// </summary>
        /// <typeparam name="TCommand">The command type</typeparam>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="context">The command context</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        Task BeforeHandleAsync<TCommand, TResult>(
            FilterCommandContext<TCommand, TResult> context, CancellationToken ct = default(CancellationToken))
            where TCommand : ICommand<TResult>;

        /// <summary>
        /// Runs before the event handler
        /// </summary>
        /// <typeparam name="TEvent">The event type</typeparam>
        /// <param name="context">The command context</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        Task BeforeHandleAsync<TEvent>(
            FilterEventContext<TEvent> context, CancellationToken ct = default(CancellationToken))
            where TEvent : IEvent;

        /// <summary>
        /// Runs after the command handler
        /// </summary>
        /// <typeparam name="TCommand">The command type</typeparam>
        /// <param name="context">The command context</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        Task AfterHandleAsync<TCommand>(
            FilterCommandContext<TCommand> context, CancellationToken ct = default(CancellationToken))
            where TCommand : ICommand;

        /// <summary>
        /// Runs after the command handler
        /// </summary>
        /// <typeparam name="TCommand">The command type</typeparam>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="context">The command context</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        Task AfterHandleAsync<TCommand, TResult>(
            FilterCommandContext<TCommand, TResult> context, CancellationToken ct = default(CancellationToken))
            where TCommand : ICommand<TResult>;

        /// <summary>
        /// Runs after the command handler
        /// </summary>
        /// <typeparam name="TEvent">The event type</typeparam>
        /// <param name="context">The command context</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        Task AfterHandleAsync<TEvent>(
            FilterEventContext<TEvent> context, CancellationToken ct = default(CancellationToken))
            where TEvent : IEvent;
    }
}
