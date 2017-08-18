using System;
using System.Threading;
using System.Threading.Tasks;
using SimpleSoft.Mediator.Internal;

namespace SimpleSoft.Mediator.Pipeline
{
    /// <summary>
    /// Filter executed when the handling of commands or events fails.
    /// </summary>
    public abstract class HandlingFailedFilter : IHandlingFailedFilter
    {
        /// <inheritdoc />
        public virtual Task CommandFailedAsync<TCommand>(TCommand cmd, Exception exception, CancellationToken ct) where TCommand : ICommand
        {
            return Helpers.CompletedTask;
        }

        /// <inheritdoc />
        public virtual Task CommandFailedAsync<TCommand, TResult>(TCommand cmd, Exception exception, CancellationToken ct) where TCommand : ICommand<TResult>
        {
            return Helpers.CompletedTask;
        }

        /// <inheritdoc />
        public virtual Task EventFailedAsync<TEvent>(TEvent evt, Exception exception, CancellationToken ct) where TEvent : IEvent
        {
            return Helpers.CompletedTask;
        }
    }
}