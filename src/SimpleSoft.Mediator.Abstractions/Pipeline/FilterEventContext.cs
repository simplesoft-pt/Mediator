using System;

namespace SimpleSoft.Mediator.Pipeline
{
    /// <summary>
    /// Context received by handler filters
    /// </summary>
    /// <typeparam name="TEvent">The event type</typeparam>
    public sealed class FilterEventContext<TEvent> where TEvent : IEvent
    {
        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="evt">The event to be handled</param>
        /// <exception cref="ArgumentNullException"></exception>
        public FilterEventContext(TEvent evt)
        {
            if (evt == null) throw new ArgumentNullException(nameof(evt));
            Event = evt;
        }

        /// <summary>
        /// The command to be handled
        /// </summary>
        public TEvent Event { get; }

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
