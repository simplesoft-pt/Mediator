#region License
// The MIT License (MIT)
// 
// Copyright (c) 2017 Simplesoft.pt
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// Broadcasts an event of a given type to all the handlers
    /// </summary>
    /// <typeparam name="TEvent">The event type</typeparam>
    public class Broadcaster<TEvent> : IBroadcaster<TEvent> where TEvent : class, IEvent
    {
        private readonly IEnumerable<IEventHandler<TEvent>> _handlers;
        private readonly List<IPipeline> _pipelines;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="handlers"></param>
        /// <param name="pipelines"></param>
        public Broadcaster(IEnumerable<IEventHandler<TEvent>> handlers, IEnumerable<IPipeline> pipelines)
        {
            _handlers = handlers ?? throw new ArgumentNullException(nameof(handlers));
            _pipelines = pipelines?.ToList() ?? throw new ArgumentNullException(nameof(pipelines));
        }

        /// <inheritdoc />
        public virtual Task BroadcastAsync(TEvent evt, CancellationToken ct)
        {
            if (evt == null) throw new ArgumentNullException(nameof(evt));

            Func<TEvent, CancellationToken, Task> next = (@event, cancellationToken) =>
            {
                if (_handlers == null)
                {
#if NET40 || NET45 || NETSTANDARD1_1
                    var t = new TaskCompletionSource<bool>();
                    t.SetResult(true);
                    return t.Task;
#else
                    return Task.CompletedTask;
#endif
                }

                var handlers = _handlers
                    .Select(handler => handler.HandleAsync(@event, cancellationToken))
                    .ToArray();

                if (handlers.Length == 0)
                {
#if NET40 || NET45 || NETSTANDARD1_1
                    var t = new TaskCompletionSource<bool>();
                    t.SetResult(true);
                    return t.Task;
#else
                    return Task.CompletedTask;
#endif
                }

                var tcs = new TaskCompletionSource<bool>();
                
                Task.Factory.ContinueWhenAll(handlers, tasks =>
                {
                    List<Exception> exceptions = null;
                    foreach (var t in tasks)
                    {
                        if (t.Exception == null)
                            continue;

                        exceptions ??= new List<Exception>(tasks.Length);
                        exceptions.Add(t.Exception.InnerException);
                    }

                    if (exceptions == null)
                        tcs.SetResult(true);
                    else
                        tcs.SetException(new AggregateException(exceptions));
                }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Current);

                return tcs.Task;
            };

            for (var i = _pipelines.Count - 1; i >= 0; i--)
            {
                var pipeline = _pipelines[i];

                var old = next;
                next = (@event, c) => pipeline.OnEventAsync(old, @event, c);
            }

            return next(evt, ct);
        }
    }
}