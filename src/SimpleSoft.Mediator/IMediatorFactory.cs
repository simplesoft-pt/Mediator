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

using System.Collections.Generic;
using SimpleSoft.Mediator.Middleware;

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// The handler factory
    /// </summary>
    public interface IMediatorFactory
    {
        /// <summary>
        /// Builds an <see cref="ICommandHandler{TCommand}"/> for a given <see cref="ICommand"/>.
        /// </summary>
        /// <typeparam name="TCommand">The command type</typeparam>
        /// <returns>The command handler or null if not found</returns>
        ICommandHandler<TCommand> BuildCommandHandlerFor<TCommand>()
            where TCommand : ICommand;

        /// <summary>
        /// Builds an <see cref="ICommandHandler{TCommand,TResult}"/> for a 
        /// given <see cref="ICommand{TResult}"/>.
        /// </summary>
        /// <typeparam name="TCommand">The command type</typeparam>
        /// <typeparam name="TResult">The command result type</typeparam>
        /// <returns>The command handler or null if not found</returns>
        ICommandHandler<TCommand, TResult> BuildCommandHandlerFor<TCommand, TResult>()
            where TCommand : ICommand<TResult>;

        /// <summary>
        /// Builds a collection of <see cref="IEventHandler{TEvent}"/> for a given <see cref="IEvent"/>.
        /// </summary>
        /// <typeparam name="TEvent">The event type</typeparam>
        /// <returns>A collection of event handlers</returns>
        IEnumerable<IEventHandler<TEvent>> BuildEventHandlersFor<TEvent>()
            where TEvent : IEvent;

        /// <summary>
        /// Builds an <see cref="IQueryHandler{TQuery,TResult}"/> for a 
        /// given <see cref="IQuery{TResult}"/>.
        /// </summary>
        /// <typeparam name="TQuery">The query type</typeparam>
        /// <typeparam name="TResult">The query result type</typeparam>
        /// <returns>The query handler or null if not found</returns>
        IQueryHandler<TQuery, TResult> BuildQueryHandlerFor<TQuery, TResult>()
            where TQuery : IQuery<TResult>;

        /// <summary>
        /// Builds a collection of all registered <see cref="ICommandMiddleware"/>.
        /// </summary>
        /// <returns>A collection of command middlewares</returns>
        IEnumerable<ICommandMiddleware> BuildCommandMiddlewares();

        /// <summary>
        /// Builds a collection of all registered <see cref="IEventMiddleware"/>.
        /// </summary>
        /// <returns>A collection of event middlewares</returns>
        IEnumerable<IEventMiddleware> BuildEventMiddlewares();

        /// <summary>
        /// Builds a collection of all registered <see cref="IQueryMiddleware"/>.
        /// </summary>
        /// <returns>A collection of query middlewares</returns>
        IEnumerable<IQueryMiddleware> BuildQueryMiddlewares();
    }
}