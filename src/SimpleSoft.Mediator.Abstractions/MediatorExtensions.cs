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

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// Helper methods for the <see cref="IMediator"/> interface
    /// </summary>
    public static class MediatorExtensions
    {
        /// <summary>
        /// Sends a command to an <see cref="ICommandHandler{TCommand}"/>.
        /// </summary>
        /// <typeparam name="TCommand">The command type</typeparam>
        /// <param name="mediator">The mediator to use</param>
        /// <param name="cmd">The command to publish</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Send<TCommand>(this IMediator mediator, TCommand cmd)
            where TCommand : class, ICommand
        {
            if (mediator == null) throw new ArgumentNullException(nameof(mediator));

            mediator.SendAsync(cmd, CancellationToken.None)
#if NET40
                .Wait();
#else
                .ConfigureAwait(false).GetAwaiter().GetResult();
#endif
        }

        /// <summary>
        /// Sends a command to an <see cref="ICommandHandler{TCommand,TResult}"/> and 
        /// returns the operation result.
        /// </summary>
        /// <typeparam name="TCommand">The command type</typeparam>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="mediator">The mediator to use</param>
        /// <param name="cmd">The command to publish</param>
        /// <returns>The handler result</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TResult Send<TCommand, TResult>(this IMediator mediator, TCommand cmd)
            where TCommand : class, ICommand<TResult>
        {
            if (mediator == null) throw new ArgumentNullException(nameof(mediator));

            return mediator.SendAsync<TCommand, TResult>(cmd, CancellationToken.None)
#if NET40
                .Result;
#else
                .ConfigureAwait(false).GetAwaiter().GetResult();
#endif
        }

        /// <summary>
        /// Broadcast the event across all <see cref="IEventHandler{TEvent}"/>.
        /// </summary>
        /// <typeparam name="TEvent">The event type</typeparam>
        /// <param name="mediator">The mediator to use</param>
        /// <param name="evt">The event to broadcast</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Broadcast<TEvent>(this IMediator mediator, TEvent evt)
            where TEvent : class, IEvent
        {
            if (mediator == null) throw new ArgumentNullException(nameof(mediator));

            mediator.BroadcastAsync(evt, CancellationToken.None)
#if NET40
                .Wait();
#else
                .ConfigureAwait(false).GetAwaiter().GetResult();
#endif
        }

        /// <summary>
        /// Fetches a query from a <see cref="IQueryHandler{TQuery,TResult}"/> and 
        /// returns the query result.
        /// </summary>
        /// <typeparam name="TQuery">The query type</typeparam>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="mediator">The mediator to use</param>
        /// <param name="query">The query to fetch</param>
        /// <returns>The handler result</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TResult Fetch<TQuery, TResult>(this IMediator mediator, TQuery query)
            where TQuery : class, IQuery<TResult>
        {
            if (mediator == null) throw new ArgumentNullException(nameof(mediator));

            return mediator.FetchAsync<TQuery, TResult>(query, CancellationToken.None)
#if NET40
                .Result;
#else
                .ConfigureAwait(false).GetAwaiter().GetResult();
#endif
        }
    }
}
