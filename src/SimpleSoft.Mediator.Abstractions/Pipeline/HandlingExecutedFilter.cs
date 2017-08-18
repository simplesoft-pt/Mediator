using System.Threading;
using System.Threading.Tasks;
using SimpleSoft.Mediator.Internal;
using TaskExtensions = SimpleSoft.Mediator.Internal.TaskExtensions;

namespace SimpleSoft.Mediator.Pipeline
{
    /// <summary>
    /// Filter run after the handling of commands or events.
    /// </summary>
    public abstract class HandlingExecutedFilter : IHandlingExecutedFilter
    {
        /// <inheritdoc />
        public virtual Task CommandExecutedAsync<TCommand>(TCommand cmd, CancellationToken ct) where TCommand : ICommand
        {
            return TaskExtensions.CompletedTask;
        }

        /// <inheritdoc />
        public virtual Task CommandExecutedAsync<TCommand, TResult>(TCommand cmd, TResult result, CancellationToken ct) where TCommand : ICommand<TResult>
        {
            return TaskExtensions.CompletedTask;
        }

        /// <inheritdoc />
        public virtual Task EventExecutedAsync<TEvent>(TEvent evt, CancellationToken ct) where TEvent : IEvent
        {
            return TaskExtensions.CompletedTask;
        }
    }
}
