using System.Threading;
using System.Threading.Tasks;

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// Represents the mediator to receive commands and events
    /// </summary>
    public interface IMediator
    {
        /// <summary>
        /// Publishes a command to an <see cref="ICommandHandler{TCommand}"/>.
        /// </summary>
        /// <typeparam name="TCommand">The command type</typeparam>
        /// <param name="cmd">The command to publish</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        Task PublishAsync<TCommand>(TCommand cmd, CancellationToken ct)
            where TCommand : ICommand;

        /// <summary>
        /// Publishes a command to an <see cref="ICommandHandler{TCommand,TResult}"/> and 
        /// returns the operation result.
        /// </summary>
        /// <typeparam name="TCommand">The command type</typeparam>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="cmd">The command to publish</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited for the result</returns>
        Task<TResult> PublishAsync<TCommand, TResult>(TCommand cmd, CancellationToken ct)
            where TCommand : ICommand<TResult>;

        /// <summary>
        /// Broadcast the event across all <see cref="IEventHandler{TEvent}"/>.
        /// </summary>
        /// <typeparam name="TEvent">The event type</typeparam>
        /// <param name="evt">The event to broadcast</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        Task BroadcastAsync<TEvent>(TEvent evt, CancellationToken ct)
            where TEvent : IEvent;
    }
}
