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
using Microsoft.Extensions.Logging;

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// Mediator wrapper with logging support
    /// </summary>
    public class MicrosoftMediator : IMediator
    {
        private readonly ILogger<MicrosoftMediator> _logger;
        private readonly IMediator _mediator;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="serviceProvider">The mediator factory</param>
        /// <param name="pipelines">The mediator pipeline collection</param>
        /// <param name="logger">The logger instance</param>
        /// <exception cref="ArgumentNullException"></exception>
        public MicrosoftMediator(IMediatorServiceProvider serviceProvider, IEnumerable<IPipeline> pipelines, ILogger<MicrosoftMediator> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = new Mediator(serviceProvider, pipelines);
        }

        /// <inheritdoc />
        public async Task SendAsync(ICommand cmd, CancellationToken ct)
        {
            if (cmd == null) throw new ArgumentNullException(nameof(cmd));

            using (_logger.BeginScope(
                "CommandName:{commandName} CommandId:{commandId}", cmd.GetType().Name, cmd.Id))
            {
                _logger.LogDebug("Sending command");
                await _mediator.SendAsync(cmd, ct).ConfigureAwait(false);
            }
        }

        /// <inheritdoc />
        public async Task<TResult> SendAsync<TResult>(ICommand<TResult> cmd, CancellationToken ct)
        {
            using (_logger.BeginScope(
                "CommandName:{commandName} CommandId:{commandId}", cmd.GetType().Name, cmd.Id))
            {
                _logger.LogDebug("Sending command");
                return await _mediator.SendAsync(cmd, ct).ConfigureAwait(false);
            }
        }

        /// <inheritdoc />
        public async Task BroadcastAsync(IEvent evt, CancellationToken ct)
        {
            if (evt == null) throw new ArgumentNullException(nameof(evt));

            using (_logger.BeginScope(
                "EventName:{eventName} EventId:{eventId}", evt.GetType().Name, evt.Id))
            {
                _logger.LogDebug("Broadcasting event");
                await _mediator.BroadcastAsync(evt, ct).ConfigureAwait(false);
            }
        }

        /// <inheritdoc />
        public async Task<TResult> FetchAsync<TResult>(IQuery<TResult> query, CancellationToken ct)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            using (_logger.BeginScope(
                "QueryName:{queryName} QueryId:{queryId}", query.GetType().Name, query.Id))
            {
                _logger.LogDebug("Fetching query data");
                return await _mediator.FetchAsync(query, ct).ConfigureAwait(false);
            }
        }
    }
}