using System;
using System.Threading;
using System.Threading.Tasks;
using TaskExtensions = SimpleSoft.Mediator.Internal.TaskExtensions;

namespace SimpleSoft.Mediator.Pipeline
{
    /// <summary>
    /// Filter that is run on every handling event
    /// </summary>
    public abstract class HandlingFilter : IHandlingFilter
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
        
        /// <inheritdoc />
        public virtual Task OnExecutedCommandAsync<TCommand>(TCommand cmd, CancellationToken ct) where TCommand : ICommand
        {
            return TaskExtensions.CompletedTask;
        }

        /// <inheritdoc />
        public virtual Task OnExecutedCommandAsync<TCommand, TResult>(TCommand cmd, TResult result, CancellationToken ct) where TCommand : ICommand<TResult>
        {
            return TaskExtensions.CompletedTask;
        }

        /// <inheritdoc />
        public virtual Task OnExecutedEventAsync<TEvent>(TEvent evt, CancellationToken ct) where TEvent : IEvent
        {
            return TaskExtensions.CompletedTask;
        }

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