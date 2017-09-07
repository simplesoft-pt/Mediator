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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SimpleSoft.Mediator.Pipeline;

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// Mediator to publish commands, broadcast events and fetch queries
    /// </summary>
    public class Mediator : IMediator
    {
        private readonly IMediatorFactory _factory;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="factory">The handler factory</param>
        /// <exception cref="ArgumentNullException"></exception>
        public Mediator(IMediatorFactory factory)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            _factory = factory;
        }

        /// <inheritdoc />
        public async Task SendAsync<TCommand>(TCommand cmd, CancellationToken ct = default(CancellationToken))
            where TCommand : ICommand
        {
            if (cmd == null) throw new ArgumentNullException(nameof(cmd));

            CommandMiddlewareDelegate<TCommand> next = async (command, cancellationToken) =>
            {
                var handler = _factory.BuildCommandHandlerFor<TCommand>();
                if (handler == null)
                    throw CommandHandlerNotFoundException.Build(cmd);
                await handler.HandleAsync(command, cancellationToken).ConfigureAwait(false);
            };

            foreach (var middleware in _factory.BuildCommandMiddlewares().Reverse())
            {
                var old = next;
                next = async (command, cancellationToken) =>
                    await middleware.OnCommandAsync(old, command, cancellationToken).ConfigureAwait(false);
            }

            await next(cmd, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<TResult> SendAsync<TCommand, TResult>(TCommand cmd, CancellationToken ct = default(CancellationToken)) 
            where TCommand : ICommand<TResult>
        {
            if (cmd == null) throw new ArgumentNullException(nameof(cmd));
            
            CommandMiddlewareDelegate<TCommand, TResult> next = async (command, cancellationToken) =>
            {
                var handler = _factory.BuildCommandHandlerFor<TCommand, TResult>();
                if (handler == null)
                    throw CommandHandlerNotFoundException.Build<TCommand, TResult>(cmd);
                return await handler.HandleAsync(command, cancellationToken).ConfigureAwait(false);
            };

            foreach (var middleware in _factory.BuildCommandMiddlewares().Reverse())
            {
                var old = next;
                next = async (command, cancellationToken) =>
                    await middleware.OnCommandAsync(old, command, cancellationToken).ConfigureAwait(false);
            }

            return await next(cmd, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task BroadcastAsync<TEvent>(TEvent evt, CancellationToken ct = default(CancellationToken))
            where TEvent : IEvent
        {
            if (evt == null) throw new ArgumentNullException(nameof(evt));

            EventMiddlewareDelegate<TEvent> next = async (@event, cancellationToken) =>
            {
                var handlers = _factory.BuildEventHandlersFor<TEvent>();
                foreach (var handler in handlers)
                    await handler.HandleAsync(@event, cancellationToken).ConfigureAwait(false);
            };

            foreach (var middleware in _factory.BuildEventMiddlewares().Reverse())
            {
                var old = next;
                next = async (@event, cancellationToken) =>
                    await middleware.OnEventAsync(old, @event, cancellationToken).ConfigureAwait(false);
            }

            await next(evt, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<TResult> FetchAsync<TQuery, TResult>(TQuery query,
            CancellationToken ct = default(CancellationToken))
            where TQuery : IQuery<TResult>
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            
            QueryMiddlewareDelegate<TQuery, TResult> next = async (q, cancellationToken) =>
            {
                var handler = _factory.BuildQueryHandlerFor<TQuery, TResult>();
                if (handler == null)
                    throw QueryHandlerNotFoundException.Build<TQuery, TResult>(query);
                return await handler.HandleAsync(q, cancellationToken).ConfigureAwait(false);
            };

            foreach (var middleware in _factory.BuildQueryMiddlewares().Reverse())
            {
                var old = next;
                next = async (q, cancellationToken) =>
                    await middleware.OnQueryAsync(old, q, cancellationToken).ConfigureAwait(false);
            }

            return await next(query, ct).ConfigureAwait(false);
        }
    }
}
