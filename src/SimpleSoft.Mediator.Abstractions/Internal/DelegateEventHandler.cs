using System;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleSoft.Mediator.Internal
{
    internal class DelegateEventHandler<TEvent> : IEventHandler<TEvent>
        where TEvent : IEvent
    {
        private readonly Func<TEvent, CancellationToken, Task> _handler;

        public DelegateEventHandler(Func<TEvent, CancellationToken, Task> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            _handler = handler;
        }

        public async Task HandleAsync(TEvent evt, CancellationToken ct)
        {
            await _handler(evt, ct).ConfigureAwait(false);
        }
    }
}
