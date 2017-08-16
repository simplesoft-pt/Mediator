using System.Threading;
using System.Threading.Tasks;

namespace SimpleSoft.Mediator.Pipeline
{
    /// <summary>
    /// Handler filter to help implement only the needed methods
    /// </summary>
    public abstract class Filter : IFilter
    {
        private static readonly Task CompletedTask = Task.FromResult(true);

        /// <inheritdoc />
        public int Order { get; } = 0;

        /// <inheritdoc />
        public virtual Task BeforeHandleAsync<TCommand>(FilterCommandContext<TCommand> context, CancellationToken ct = new CancellationToken()) where TCommand : ICommand
        {
            return CompletedTask;
        }

        /// <inheritdoc />
        public virtual Task BeforeHandleAsync<TCommand, TResult>(FilterCommandContext<TCommand, TResult> context, CancellationToken ct = new CancellationToken()) where TCommand : ICommand<TResult>
        {
            return CompletedTask;
        }

        /// <inheritdoc />
        public virtual Task BeforeHandleAsync<TEvent>(FilterEventContext<TEvent> context, CancellationToken ct = new CancellationToken()) where TEvent : IEvent
        {
            return CompletedTask;
        }

        /// <inheritdoc />
        public virtual Task AfterHandleAsync<TCommand>(FilterCommandContext<TCommand> context, CancellationToken ct = new CancellationToken()) where TCommand : ICommand
        {
            return CompletedTask;
        }

        /// <inheritdoc />
        public virtual Task AfterHandleAsync<TCommand, TResult>(FilterCommandContext<TCommand, TResult> context, CancellationToken ct = new CancellationToken()) where TCommand : ICommand<TResult>
        {
            return CompletedTask;
        }

        /// <inheritdoc />
        public virtual Task AfterHandleAsync<TEvent>(FilterEventContext<TEvent> context, CancellationToken ct = new CancellationToken()) where TEvent : IEvent
        {
            return CompletedTask;
        }
    }
}