using System;
using System.Threading;
using System.Threading.Tasks;
using TaskExtensions = SimpleSoft.Mediator.Internal.TaskExtensions;

namespace SimpleSoft.Mediator.Pipeline
{
    /// <summary>
    /// Filter executed when the handling of commands or events fails.
    /// </summary>
    public abstract class HandlingFailedFilter : IHandlingFailedFilter
    {
        /// <inheritdoc />
        public virtual Task OnFailedCommandAsync<TCommand>(TCommand cmd, Exception exception, CancellationToken ct) where TCommand : ICommand
        {
            return TaskExtensions.CompletedTask;
        }

        /// <inheritdoc />
        public virtual Task OnFailedCommandAsync<TCommand, TResult>(TCommand cmd, Exception exception, CancellationToken ct) where TCommand : ICommand<TResult>
        {
            return TaskExtensions.CompletedTask;
        }

        /// <inheritdoc />
        public virtual Task OnFailedEventAsync<TEvent>(TEvent evt, Exception exception, CancellationToken ct) where TEvent : IEvent
        {
            return TaskExtensions.CompletedTask;
        }
    }
}