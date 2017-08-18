using System.Threading;
using System.Threading.Tasks;
using TaskExtensions = SimpleSoft.Mediator.Internal.TaskExtensions;

namespace SimpleSoft.Mediator.Pipeline
{
    /// <summary>
    /// Filter run before the handling of commands or events.
    /// </summary>
    public abstract class HandlingExecutingFilter : IHandlingExecutingFilter
    {
        /// <inheritdoc />
        public virtual Task OnExecutingCommandAsync<TCommand>(TCommand cmd, CancellationToken ct) where TCommand : ICommand
        {
            return TaskExtensions.CompletedTask;
        }

        /// <inheritdoc />
        public virtual Task OnExecutingCommandAsync<TCommand, TResult>(TCommand cmd, CancellationToken ct) where TCommand : ICommand<TResult>
        {
            return TaskExtensions.CompletedTask;
        }

        /// <inheritdoc />
        public virtual Task OnExecutingEventAsync<TEvent>(TEvent evt, CancellationToken ct) where TEvent : IEvent
        {
            return TaskExtensions.CompletedTask;
        }
    }
}