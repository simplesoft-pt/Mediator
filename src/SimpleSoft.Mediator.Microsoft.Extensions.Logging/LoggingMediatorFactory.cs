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
using Microsoft.Extensions.Logging;
using SimpleSoft.Mediator.Pipeline;

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// Mediator factory that uses delegates to build the required services
    /// while making some helpful logs
    /// </summary>
    public class LoggingMediatorFactory : IMediatorFactory
    {
        private readonly IMediatorFactory _factory;
        private readonly ILogger<LoggingMediatorFactory> _logger;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="factory">The factory to be wrapped</param>
        /// <param name="logger">The logger to use</param>
        /// <exception cref="ArgumentNullException"></exception>
        public LoggingMediatorFactory(IMediatorFactory factory, ILogger<LoggingMediatorFactory> logger)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            if (logger == null) throw new ArgumentNullException(nameof(logger));

            _factory = factory;
            _logger = logger;
        }

        /// <inheritdoc />
        public ICommandHandler<TCommand> BuildCommandHandlerFor<TCommand>() where TCommand : ICommand
        {
            _logger.LogDebug("Building command handler");
            return _factory.BuildCommandHandlerFor<TCommand>();
        }

        /// <inheritdoc />
        public ICommandHandler<TCommand, TResult> BuildCommandHandlerFor<TCommand, TResult>() where TCommand : ICommand<TResult>
        {
            _logger.LogDebug("Building command handler");
            return _factory.BuildCommandHandlerFor<TCommand, TResult>();
        }

        /// <inheritdoc />
        public IEnumerable<IEventHandler<TEvent>> BuildEventHandlersFor<TEvent>() where TEvent : IEvent
        {
            _logger.LogDebug("Building event handlers");
            return _factory.BuildEventHandlersFor<TEvent>();
        }

        /// <inheritdoc />
        public IQueryHandler<TQuery, TResult> BuildQueryHandlerFor<TQuery, TResult>() where TQuery : IQuery<TResult>
        {
            _logger.LogDebug("Building query handler");
            return _factory.BuildQueryHandlerFor<TQuery, TResult>();
        }

        /// <inheritdoc />
        public IEnumerable<ICommandMiddleware> BuildCommandMiddlewares()
        {
            _logger.LogDebug("Building command middleware collection");
            return _factory.BuildCommandMiddlewares();
        }

        /// <inheritdoc />
        public IEnumerable<IEventMiddleware> BuildEventMiddlewares()
        {
            _logger.LogDebug("Building event middleware collection");
            return _factory.BuildEventMiddlewares();
        }

        /// <inheritdoc />
        public IEnumerable<IQueryMiddleware> BuildQueryMiddlewares()
        {
            _logger.LogDebug("Building query middleware collection");
            return _factory.BuildQueryMiddlewares();
        }

        /// <summary>
        /// Wrapper implementation using an instance of <see cref="DelegateMediatorFactory"/>.
        /// </summary>
        public class Delegate : LoggingMediatorFactory
        {
            /// <summary>
            /// Creates a new instance
            /// </summary>
            /// <param name="serviceFactory">The factory for single services</param>
            /// <param name="serviceCollectionFactory">The factory for collections of services</param>
            /// <param name="logger">The logger to use</param>
            /// <exception cref="ArgumentNullException"></exception>
            public Delegate(
                DelegateMediatorFactory.Service serviceFactory,
                DelegateMediatorFactory.ServiceCollection serviceCollectionFactory,
                ILogger<LoggingMediatorFactory> logger)
                : base(new DelegateMediatorFactory(serviceFactory, serviceCollectionFactory), logger)
            {

            }
        }
    }
}
