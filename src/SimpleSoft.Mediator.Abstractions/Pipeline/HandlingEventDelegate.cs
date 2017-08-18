using System.Threading;
using System.Threading.Tasks;

namespace SimpleSoft.Mediator.Pipeline
{
    /// <summary>
    /// Method invoked when an <see cref="IEvent"/> is broadcast.
    /// </summary>
    /// <typeparam name="TEvent">The event type</typeparam>
    /// <param name="evt">The event broadcasted</param>
    /// <param name="ct">The cancellation token</param>
    /// <returns>A task to be awaited</returns>
    public delegate Task HandlingEventDelegate<in TEvent>(TEvent evt, CancellationToken ct)
        where TEvent : IEvent;
}