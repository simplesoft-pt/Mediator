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
using SimpleSoft.Mediator.Middleware;

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// Middleware that adds logger scopes on each command, event and querie.
    /// When using the Logging.<see cref="LoggingMediator"/> wrapper this middleware
    /// is not required.
    /// </summary>
    public class LoggingMiddleware : ICommandMiddleware, IEventMiddleware, IQueryMiddleware
    {
        private readonly ILogger<LoggingMiddleware> _logger;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="logger">The logger to be used</param>
        /// <exception cref="ArgumentNullException"></exception>
        public LoggingMiddleware(ILogger<LoggingMiddleware> logger)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task OnCommandAsync<TCommand>(CommandMiddlewareDelegate<TCommand> next, TCommand cmd, CancellationToken ct) where TCommand : ICommand
        {
            using (_logger.BeginScope(
                "CommandName:{commandName} CommandId:{commandId}", typeof(TCommand).Name, cmd.Id))
            {
                _logger.LogDebug("Publishing command");
                await next(cmd, ct).ConfigureAwait(false);
            }
        }

        /// <inheritdoc />
        public async Task<TResult> OnCommandAsync<TCommand, TResult>(CommandMiddlewareDelegate<TCommand, TResult> next, TCommand cmd, CancellationToken ct) where TCommand : ICommand<TResult>
        {
            using (_logger.BeginScope(
                "CommandName:{commandName} CommandId:{commandId}", typeof(TCommand).Name, cmd.Id))
            {
                _logger.LogDebug("Publishing command with result");
                return await next(cmd, ct).ConfigureAwait(false);
            }
        }

        /// <inheritdoc />
        public async Task OnEventAsync<TEvent>(EventMiddlewareDelegate<TEvent> next, TEvent evt, CancellationToken ct) where TEvent : IEvent
        {
            using (_logger.BeginScope(
                "EventName:{eventName} EventId:{eventId}", typeof(TEvent).Name, evt.Id))
            {
                _logger.LogDebug("Broadcasting event");
                await next(evt, ct).ConfigureAwait(false);
            }
        }

        /// <inheritdoc />
        public async Task<TResult> OnQueryAsync<TQuery, TResult>(QueryMiddlewareDelegate<TQuery, TResult> next, TQuery query, CancellationToken ct) where TQuery : IQuery<TResult>
        {
            using (_logger.BeginScope(
                "QueryName:{queryName} QueryId:{queryId}", typeof(TQuery).Name, query.Id))
            {
                _logger.LogDebug("Fetching query data");
                return await next(query, ct).ConfigureAwait(false);
            }
        }
    }
}