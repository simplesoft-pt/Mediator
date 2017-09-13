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
using SimpleSoft.Mediator.Pipeline;

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// Factory for mediator dependencies that build services directly from
    /// the <see cref="IServiceProvider"/> instance.
    /// </summary>
    public class MicrosoftMediatorFactory : IMediatorFactory
    {
        private readonly IServiceProvider _provider;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="provider">The service provider</param>
        /// <exception cref="ArgumentNullException"></exception>
        public MicrosoftMediatorFactory(IServiceProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        /// <inheritdoc />
        public ICommandHandler<TCommand> BuildCommandHandlerFor<TCommand>() where TCommand : ICommand
        {
            return _provider.GetService<ICommandHandler<TCommand>>();
        }

        /// <inheritdoc />
        public ICommandHandler<TCommand, TResult> BuildCommandHandlerFor<TCommand, TResult>() where TCommand : ICommand<TResult>
        {
            return _provider.GetService<ICommandHandler<TCommand, TResult>>();
        }

        /// <inheritdoc />
        public IEnumerable<IEventHandler<TEvent>> BuildEventHandlersFor<TEvent>() where TEvent : IEvent
        {
            return _provider.GetServices<IEventHandler<TEvent>>();
        }

        /// <inheritdoc />
        public IQueryHandler<TQuery, TResult> BuildQueryHandlerFor<TQuery, TResult>() where TQuery : IQuery<TResult>
        {
            return _provider.GetService<IQueryHandler<TQuery, TResult>>();
        }

        /// <inheritdoc />
        public IEnumerable<ICommandMiddleware> BuildCommandMiddlewares()
        {
            return _provider.GetServices<ICommandMiddleware>();
        }

        /// <inheritdoc />
        public IEnumerable<IEventMiddleware> BuildEventMiddlewares()
        {
            return _provider.GetServices<IEventMiddleware>();
        }

        /// <inheritdoc />
        public IEnumerable<IQueryMiddleware> BuildQueryMiddlewares()
        {
            return _provider.GetServices<IQueryMiddleware>();
        }
    }
}
