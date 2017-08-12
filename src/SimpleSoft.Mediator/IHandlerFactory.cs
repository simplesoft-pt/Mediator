using System.Collections.Generic;

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// The handler factory
    /// </summary>
    public interface IHandlerFactory
    {
        /// <summary>
        /// Builds an <see cref="ICommandHandler{TCommand}"/> for a given <see cref="ICommand"/>.
        /// </summary>
        /// <typeparam name="TCommand">The command type</typeparam>
        /// <returns>The command handler or null if not found</returns>
        ICommandHandler<TCommand> BuildCommandHandlerFor<TCommand>()
            where TCommand : ICommand;

        /// <summary>
        /// Builds an <see cref="ICommandHandler{TCommand,TResult}"/> for a 
        /// given <see cref="ICommand{TResult}"/>.
        /// </summary>
        /// <typeparam name="TCommand">The command type</typeparam>
        /// <typeparam name="TResult">The command result type</typeparam>
        /// <returns>The command handler or null if not found</returns>
        ICommandHandler<TCommand, TResult> BuildCommandHandlerFor<TCommand, TResult>()
            where TCommand : ICommand<TResult>;

        /// <summary>
        /// Builds a collection of <see cref="IEventHandler{TEvent}"/> for a given <see cref="IEvent"/>.
        /// </summary>
        /// <typeparam name="TEvent">The event type</typeparam>
        /// <returns>A collection of event handlers</returns>
        IEnumerable<IEventHandler<TEvent>> BuildEventHandlersFor<TEvent>()
            where TEvent : IEvent;
    }
}