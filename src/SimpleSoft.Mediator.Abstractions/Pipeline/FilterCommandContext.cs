using System;

namespace SimpleSoft.Mediator.Pipeline
{
    /// <summary>
    /// Context received by handler filters
    /// </summary>
    /// <typeparam name="TCommand">The command type</typeparam>
    public sealed class FilterCommandContext<TCommand> where TCommand : ICommand
    {
        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="cmd">The command to be handled</param>
        /// <exception cref="ArgumentNullException"></exception>
        public FilterCommandContext(TCommand cmd)
        {
            if (cmd == null) throw new ArgumentNullException(nameof(cmd));
            Command = cmd;
        }

        /// <summary>
        /// The command to be handled
        /// </summary>
        public TCommand Command { get; }

        /// <summary>
        /// Exception thrown by the handler, if any. To ignore the exception,
        /// set it to null.
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Did the handler thrown an exception? 
        /// False if <see cref="Exception"/> is null, otherwise true
        /// </summary>
        public bool Failed => Exception != null;
    }

    /// <summary>
    /// Context received by handler filters
    /// </summary>
    /// <typeparam name="TCommand">The command type</typeparam>
    /// <typeparam name="TResult">The command result type</typeparam>
    public sealed class FilterCommandContext<TCommand, TResult> where TCommand : ICommand<TResult>
    {
        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="command">The command to be handled</param>
        /// <exception cref="ArgumentNullException"></exception>
        public FilterCommandContext(TCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            Command = command;
        }

        /// <summary>
        /// The command to be handled
        /// </summary>
        public TCommand Command { get; }

        /// <summary>
        /// The command result
        /// </summary>
        public TResult Result { get; set; }

        /// <summary>
        /// Exception thrown by the handler, if any. To ignore the exception,
        /// set it to null.
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Did the handler thrown an exception? 
        /// False if <see cref="Exception"/> is null, otherwise true
        /// </summary>
        public bool Failed => Exception != null;
    }
}