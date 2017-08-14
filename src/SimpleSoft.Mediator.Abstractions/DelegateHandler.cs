using System;
using System.Threading;
using System.Threading.Tasks;
using SimpleSoft.Mediator.Internal;

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// Builds handlers using delegate functions
    /// </summary>
    public static class DelegateHandler
    {
        /// <summary>
        /// Builds a command handler that wraps the given delegate.
        /// </summary>
        /// <typeparam name="TCommand">The command type</typeparam>
        /// <param name="handler">The delegate to wrap</param>
        /// <returns>The command handler</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static ICommandHandler<TCommand> Command<TCommand>(Func<TCommand, CancellationToken, Task> handler)
            where TCommand : ICommand
        {
            return new DelegateCommandHandler<TCommand>(handler);
        }

        /// <summary>
        /// Builds a command handler that wraps the given delegate.
        /// </summary>
        /// <typeparam name="TCommand">The command type</typeparam>
        /// <typeparam name="TResult">The command result type</typeparam>
        /// <param name="handler">The delegate to wrap</param>
        /// <returns>The command handler</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static ICommandHandler<TCommand, TResult> Command<TCommand, TResult>(Func<TCommand, CancellationToken, Task<TResult>> handler)
            where TCommand : ICommand<TResult>
        {
            return new DelegateCommandHandler<TCommand, TResult>(handler);
        }

        /// <summary>
        /// Builds an event handler that wraps the given delegate.
        /// </summary>
        /// <typeparam name="TEvent">The event type</typeparam>
        /// <param name="handler">The delegate to wrap</param>
        /// <returns>The event handler</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEventHandler<TEvent> Event<TEvent>(Func<TEvent, CancellationToken, Task> handler)
            where TEvent : IEvent
        {
            return new DelegateEventHandler<TEvent>(handler);
        }
    }
}
