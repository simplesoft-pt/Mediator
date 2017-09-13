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
using SimpleSoft.Mediator.Pipeline;

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// Mediator factory that uses delegates to build the required services
    /// </summary>
    public class DelegateMediatorFactory : IMediatorFactory
    {
        private static readonly IEnumerable<ICommandMiddleware> EmptyCommandMiddlewares =
            Enumerable.Empty<ICommandMiddleware>();
        private static readonly IEnumerable<IEventMiddleware> EmptyEventMiddlewares =
            Enumerable.Empty<IEventMiddleware>();
        private static readonly IEnumerable<IQueryMiddleware> EmptyQueryMiddlewares =
            Enumerable.Empty<IQueryMiddleware>();

        private readonly Service _serviceFactory;
        private readonly ServiceCollection _serviceCollectionFactory;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="serviceFactory">The factory for single services</param>
        /// <param name="serviceCollectionFactory">The factory for collections of services</param>
        /// <exception cref="ArgumentNullException"></exception>
        public DelegateMediatorFactory(Service serviceFactory, ServiceCollection serviceCollectionFactory)
        {
            _serviceFactory = serviceFactory ?? throw new ArgumentNullException(nameof(serviceFactory));
            _serviceCollectionFactory = serviceCollectionFactory ?? throw new ArgumentNullException(nameof(serviceCollectionFactory));
        }

        /// <inheritdoc />
        public ICommandHandler<TCommand> BuildCommandHandlerFor<TCommand>() 
            where TCommand : ICommand
        {
            var service = _serviceFactory(typeof(ICommandHandler<TCommand>));

            return (ICommandHandler<TCommand>) service;
        }

        /// <inheritdoc />
        public ICommandHandler<TCommand, TResult> BuildCommandHandlerFor<TCommand, TResult>()
            where TCommand : ICommand<TResult>
        {
            var service = _serviceFactory(typeof(ICommandHandler<TCommand, TResult>));

            return (ICommandHandler<TCommand, TResult>) service;
        }

        /// <inheritdoc />
        public IEnumerable<IEventHandler<TEvent>> BuildEventHandlersFor<TEvent>() 
            where TEvent : IEvent
        {
            var services = _serviceCollectionFactory(typeof(IEventHandler<TEvent>));

            return services?.Cast<IEventHandler<TEvent>>() ?? Enumerable.Empty<IEventHandler<TEvent>>();
        }

        /// <inheritdoc />
        public IQueryHandler<TQuery, TResult> BuildQueryHandlerFor<TQuery, TResult>()
            where TQuery : IQuery<TResult>
        {
            var service = _serviceFactory(typeof(IQueryHandler<TQuery, TResult>));

            return (IQueryHandler<TQuery, TResult>) service;
        }

        /// <inheritdoc />
        public IEnumerable<ICommandMiddleware> BuildCommandMiddlewares()
        {
            var services = _serviceCollectionFactory(typeof(ICommandMiddleware));

            return services?.Cast<ICommandMiddleware>() ?? EmptyCommandMiddlewares;
        }

        /// <inheritdoc />
        public IEnumerable<IEventMiddleware> BuildEventMiddlewares()
        {
            var services = _serviceCollectionFactory(typeof(IEventMiddleware));

            return services?.Cast<IEventMiddleware>() ?? EmptyEventMiddlewares;
        }

        /// <inheritdoc />
        public IEnumerable<IQueryMiddleware> BuildQueryMiddlewares()
        {
            var services = _serviceCollectionFactory(typeof(IQueryMiddleware));

            return services?.Cast<IQueryMiddleware>() ?? EmptyQueryMiddlewares;
        }

        /// <summary>
        /// Delegate used to resolve handler services.
        /// </summary>
        /// <param name="serviceType">The service type</param>
        /// <returns>The resolved instance</returns>
        public delegate object Service(Type serviceType);

        /// <summary>
        /// Delegate used to resolve a collection of services.
        /// </summary>
        /// <param name="serviceType">The service type</param>
        /// <returns>The collection of resolved instances</returns>
        public delegate IEnumerable<object> ServiceCollection(Type serviceType);
    }
}
