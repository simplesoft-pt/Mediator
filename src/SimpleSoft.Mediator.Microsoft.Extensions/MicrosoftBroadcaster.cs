using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// Broadcasts an event of a given type to all the handlers
    /// </summary>
    /// <typeparam name="TEvent">The event type</typeparam>
    public class MicrosoftBroadcaster<TEvent> : Broadcaster<TEvent> where TEvent : class, IEvent
    {
        private readonly ILogger<Broadcaster<TEvent>> _logger;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="handlers"></param>
        /// <param name="pipelines"></param>
        /// <param name="logger"></param>
        public MicrosoftBroadcaster(
            IEnumerable<IEventHandler<TEvent>> handlers, 
            IEnumerable<IPipeline> pipelines,
            ILogger<Broadcaster<TEvent>> logger) 
            : base(handlers, pipelines)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public override async Task BroadcastAsync(TEvent evt, CancellationToken ct)
        {
            using (_logger.BeginScope("EventName:{eventName} EventId:{eventId}", evt.GetType().Name, evt.Id))
            {
                _logger.LogDebug("Broadcasting event");
                await base.BroadcastAsync(evt, ct);
            }
        }
    }
}
