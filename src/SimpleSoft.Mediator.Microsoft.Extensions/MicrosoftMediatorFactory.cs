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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// Factory for mediator dependencies that build services directly from
    /// the <see cref="IServiceProvider"/> instance.
    /// </summary>
    public class MicrosoftMediatorFactory : IMediatorFactory
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger<MicrosoftMediatorFactory> _logger;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="provider">The service provider</param>
        /// <param name="logger">The factory logger</param>
        /// <exception cref="ArgumentNullException"></exception>
        public MicrosoftMediatorFactory(IServiceProvider provider, ILogger<MicrosoftMediatorFactory> logger)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public ICommandHandler<TCommand> BuildCommandHandlerFor<TCommand>() where TCommand : ICommand
        {
            if (_logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("Building command handler for '{commandType}'", typeof(TCommand));
            return _provider.GetService<ICommandHandler<TCommand>>();
        }

        /// <inheritdoc />
        public ICommandHandler<TCommand, TResult> BuildCommandHandlerFor<TCommand, TResult>() where TCommand : ICommand<TResult>
        {
            if (_logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug(
                    "Building command handler for '{commandType}<{resultType}>'", typeof(TCommand), typeof(TResult));
            return _provider.GetService<ICommandHandler<TCommand, TResult>>();
        }

        /// <inheritdoc />
        public IEnumerable<IEventHandler<TEvent>> BuildEventHandlersFor<TEvent>() where TEvent : IEvent
        {
            if (_logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("Building event handlers for '{eventType}'", typeof(TEvent));
            return _provider.GetServices<IEventHandler<TEvent>>();
        }

        /// <inheritdoc />
        public IQueryHandler<TQuery, TResult> BuildQueryHandlerFor<TQuery, TResult>() where TQuery : IQuery<TResult>
        {
            if (_logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug(
                    "Building query handler for '{queryType}<{resultType}>'", typeof(TQuery), typeof(TResult));
            return _provider.GetService<IQueryHandler<TQuery, TResult>>();
        }
    }
}
