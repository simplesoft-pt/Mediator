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
using System.Threading;
using System.Threading.Tasks;

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// Mediator to publish commands, broadcast events and fetch queries
    /// </summary>
    public class Mediator : IMediator
    {
        private static readonly List<IPipeline> EmptyPipelines = new List<IPipeline>(0);

        private readonly IMediatorServiceProvider _serviceProvider;
        private readonly List<IPipeline> _reversedPipelines;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="serviceProvider">The handler factory</param>
        /// <param name="pipelines">The mediator pipeline collection</param>
        /// <exception cref="ArgumentNullException"></exception>
        public Mediator(IMediatorServiceProvider serviceProvider, IEnumerable<IPipeline> pipelines)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            if (pipelines == null)
                _reversedPipelines = EmptyPipelines;
            else
            {
                _reversedPipelines = new List<IPipeline>(pipelines);
                _reversedPipelines.Reverse();
            }
        }

        /// <inheritdoc />
        public async Task SendAsync<TCommand>(TCommand cmd, CancellationToken ct = default)
            where TCommand : class, ICommand
        {
            if (cmd == null) throw new ArgumentNullException(nameof(cmd));

            Func<TCommand, CancellationToken, Task> next = async (c, cancellationToken) =>
            {
                var handler = _serviceProvider.BuildService<ICommandHandler<TCommand>>();
                if (handler == null)
                    throw CommandHandlerNotFoundException.Build(c);
                await handler.HandleAsync(c, cancellationToken).ConfigureAwait(false);
            };

            foreach (var middleware in _reversedPipelines)
            {
                var old = next;
                next = async (c, cancellationToken) =>
                    await middleware.OnCommandAsync(old, c, cancellationToken).ConfigureAwait(false);
            }

            await next(cmd, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<TResult> SendAsync<TCommand, TResult>(TCommand cmd, CancellationToken ct = default) 
            where TCommand : class, ICommand<TResult>
        {
            if (cmd == null) throw new ArgumentNullException(nameof(cmd));

            Func<TCommand, CancellationToken, Task<TResult>> next = async (c, cancellationToken) =>
            {
                var handler = _serviceProvider.BuildService<ICommandHandler<TCommand, TResult>>();
                if (handler == null)
                    throw CommandHandlerNotFoundException.Build<TCommand, TResult>(c);
                return await handler.HandleAsync(c, cancellationToken).ConfigureAwait(false);
            };

            foreach (var middleware in _reversedPipelines)
            {
                var old = next;
                next = async (c, cancellationToken) =>
                    await middleware.OnCommandAsync(old, c, cancellationToken).ConfigureAwait(false);
            }

            return await next(cmd, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task BroadcastAsync<TEvent>(TEvent evt, CancellationToken ct = default)
            where TEvent : class, IEvent
        {
            if (evt == null) throw new ArgumentNullException(nameof(evt));

            Func<TEvent, CancellationToken, Task> next = async (e, cancellationToken) =>
            {
                var handlers = _serviceProvider.BuildServices<IEventHandler<TEvent>>();
                foreach (var handler in handlers)
                    await handler.HandleAsync(e, cancellationToken).ConfigureAwait(false);
            };

            foreach (var middleware in _reversedPipelines)
            {
                var old = next;
                next = async (e, cancellationToken) =>
                    await middleware.OnEventAsync(old, e, cancellationToken).ConfigureAwait(false);
            }

            await next(evt, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<TResult> FetchAsync<TQuery, TResult>(TQuery query, CancellationToken ct = default)
            where TQuery : class, IQuery<TResult>
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            
            Func<TQuery, CancellationToken, Task<TResult>> next = async (q, cancellationToken) =>
            {
                var handler = _serviceProvider.BuildService<IQueryHandler<TQuery, TResult>>();
                if (handler == null)
                    throw QueryHandlerNotFoundException.Build<TQuery, TResult>(q);
                return await handler.HandleAsync(q, cancellationToken).ConfigureAwait(false);
            };

            foreach (var middleware in _reversedPipelines)
            {
                var old = next;
                next = async (q, cancellationToken) =>
                    await middleware.OnQueryAsync(old, q, cancellationToken).ConfigureAwait(false);
            }

            return await next(query, ct).ConfigureAwait(false);
        }
    }
}
