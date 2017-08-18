using System.Threading;
using System.Threading.Tasks;

namespace SimpleSoft.Mediator.Pipeline
{
    /// <summary>
    /// Method invoked when an <see cref="ICommand"/> is published.
    /// </summary>
    /// <typeparam name="TCommand">The command type</typeparam>
    /// <param name="cmd">The command published</param>
    /// <param name="ct">The cancellation token</param>
    /// <returns>A task to be awaited</returns>
    public delegate Task HandlingCommandDelegate<in TCommand>(TCommand cmd, CancellationToken ct)
        where TCommand : ICommand;

    /// <summary>
    /// Method invoked when an <see cref="ICommand{TResult}"/> is published.
    /// </summary>
    /// <typeparam name="TCommand">The command type</typeparam>
    /// <typeparam name="TResult">The result type</typeparam>
    /// <param name="cmd">The command published</param>
    /// <param name="ct">The cancellation token</param>
    /// <returns>A task to be awaited for the result</returns>
    public delegate Task<TResult> HandlingCommandDelegate<in TCommand, TResult>(TCommand cmd, CancellationToken ct)
        where TCommand : ICommand<TResult>;
}