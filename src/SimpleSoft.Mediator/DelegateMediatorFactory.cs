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

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// Mediator factory that uses delegates to build the required services
    /// </summary>
    public class DelegateMediatorFactory : IMediatorFactory
    {
        private readonly Func<Type, object> _serviceFactory;
        private readonly Func<Type, IEnumerable<object>> _serviceCollectionFactory;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="serviceFactory">The factory for single services</param>
        /// <param name="serviceCollectionFactory">The factory for collections of services</param>
        /// <exception cref="ArgumentNullException"></exception>
        public DelegateMediatorFactory(Func<Type, object> serviceFactory, Func<Type, IEnumerable<object>> serviceCollectionFactory)
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
    }
}
