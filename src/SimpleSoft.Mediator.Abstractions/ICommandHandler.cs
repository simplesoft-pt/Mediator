using System.Threading;
using System.Threading.Tasks;

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// Represents a command handler
    /// </summary>
    /// <typeparam name="TCommand">The command type</typeparam>
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        /// <summary>
        /// Handles the given command
        /// </summary>
        /// <param name="cmd">The command to handle</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        Task HandleAsync(TCommand cmd, CancellationToken ct);
    }

    /// <summary>
    /// Represents a command handler
    /// </summary>
    /// <typeparam name="TCommand">The command type</typeparam>
    /// <typeparam name="TResult">The result type</typeparam>
    public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand<TResult>
    {
        /// <summary>
        /// Handles the given command
        /// </summary>
        /// <param name="cmd">The command to handle</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited for the result</returns>
        Task<TResult> HandleAsync(TCommand cmd, CancellationToken ct);
    }
}