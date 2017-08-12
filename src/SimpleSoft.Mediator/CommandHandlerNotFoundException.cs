using System;

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// Exception thrown when the handler for a given <see cref="ICommand"/>
    /// or <see cref="ICommand{TResult}"/> can't be found.
    /// </summary>
    public class CommandHandlerNotFoundException : InvalidOperationException
    {
        internal CommandHandlerNotFoundException(Type commandType)
        {
            CommandType = commandType;
        }

        /// <summary>
        /// The command type
        /// </summary>
        public Type CommandType { get; }

        /// <summary>
        /// The command name
        /// </summary>
        public string CommandName => CommandType.Name;

        /// <summary>
        /// Builds a new exception.
        /// </summary>
        /// <typeparam name="TCommand">The command type</typeparam>
        /// <returns>The exception instance</returns>
        public static CommandHandlerNotFoundException Build<TCommand>() where TCommand : ICommand
        {
            return new CommandHandlerNotFoundException(typeof(TCommand));
        }

        /// <summary>
        /// Builds a new exception.
        /// </summary>
        /// <typeparam name="TCommand">The command type</typeparam>
        /// <typeparam name="TResult">The command result type</typeparam>
        /// <returns>The exception instance</returns>
        public static CommandHandlerNotFoundException Build<TCommand, TResult>() where TCommand : ICommand<TResult>
        {
            return new CommandHandlerNotFoundException(typeof(TCommand));
        }
    }
}