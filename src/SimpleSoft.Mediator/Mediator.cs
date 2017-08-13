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
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// The mediator to publish commands and broadcast events
    /// </summary>
    public class Mediator : IMediator
    {
        private readonly ILogger<Mediator> _logger;
        private readonly IHandlerFactory _factory;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="logger">The logger factory</param>
        /// <param name="factory">The handler factory</param>
        /// <exception cref="ArgumentNullException"></exception>
        public Mediator(ILogger<Mediator> logger, IHandlerFactory factory)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            _logger = logger;
            _factory = factory;
        }

        /// <inheritdoc />
        public async Task PublishAsync<TCommand>(TCommand cmd, CancellationToken ct) where TCommand : ICommand
        {
            if (cmd == null) throw new ArgumentNullException(nameof(cmd));
            
            using (_logger.BeginScope(
                "CommandName:{commandName} CommandId:{commandId}", typeof(TCommand).Name, cmd.Id))
            {
                _logger.LogDebug("Building command handler");
                var handler = _factory.BuildCommandHandlerFor<TCommand>();
                if (handler == null)
                    throw CommandHandlerNotFoundException.Build(cmd);

                _logger.LogDebug("Invoking command handler");
                await handler.HandleAsync(cmd, ct).ConfigureAwait(false);
            }
        }

        /// <inheritdoc />
        public async Task<TResult> PublishAsync<TCommand, TResult>(TCommand cmd, CancellationToken ct) where TCommand : ICommand<TResult>
        {
            if (cmd == null) throw new ArgumentNullException(nameof(cmd));
            
            using (_logger.BeginScope(
                "CommandName:{commandName} CommandId:{commandId}", typeof(TCommand).Name, cmd.Id))
            {
                _logger.LogDebug("Building command handler");
                var handler = _factory.BuildCommandHandlerFor<TCommand, TResult>();
                if (handler == null)
                    throw CommandHandlerNotFoundException.Build<TCommand, TResult>(cmd);

                _logger.LogDebug("Invoking command handler");
                return await handler.HandleAsync(cmd, ct).ConfigureAwait(false);
            }
        }

        /// <inheritdoc />
        public async Task BroadcastAsync<TEvent>(TEvent evt, CancellationToken ct) where TEvent : IEvent
        {
            if (evt == null) throw new ArgumentNullException(nameof(evt));

            using (_logger.BeginScope(
                "EventName:{eventName} EventId:{eventId}", typeof(TEvent).Name, evt.Id))
            {
                _logger.LogDebug("Building event handlers collection");
                var handlers = _factory.BuildEventHandlersFor<TEvent>();
                if (handlers == null)
                {
                    _logger.LogWarning("No event handlers found. Collection was null");
                    return;
                }

                var handlerCollectionIsEmpty = true;
                _logger.LogDebug("Invoking event handlers");
                foreach (var handler in handlers)
                {
                    handlerCollectionIsEmpty = false;
                    await handler.HandleAsync(evt, ct).ConfigureAwait(false);
                }

                if (handlerCollectionIsEmpty)
                    _logger.LogWarning("No event handlers found. Collection is empty");
            }
        }
    }
}
