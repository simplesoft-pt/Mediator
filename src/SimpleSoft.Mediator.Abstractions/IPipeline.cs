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

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// Handling middleware that can be used to intercept commands, events and queries
    /// </summary>
    public interface IPipeline
    {
        /// <summary>
        /// Method invoked when an <see cref="ICommand"/> is sent.
        /// </summary>
        /// <typeparam name="TCommand">The command type</typeparam>
        /// <param name="next">The next middleware into the chain</param>
        /// <param name="cmd">The command sent</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        Task OnCommandAsync<TCommand>(Func<TCommand, CancellationToken, Task> next, TCommand cmd, CancellationToken ct)
            where TCommand : class, ICommand;

        /// <summary>
        /// Method invoked when an <see cref="ICommand{TResult}"/> is sent.
        /// </summary>
        /// <typeparam name="TCommand">The command type</typeparam>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="next">The next middleware into the chain</param>
        /// <param name="cmd">The command sent</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited for the result</returns>
        Task<TResult> OnCommandAsync<TCommand, TResult>(Func<TCommand, CancellationToken, Task<TResult>> next, TCommand cmd, CancellationToken ct)
            where TCommand : class, ICommand<TResult>;

        /// <summary>
        /// Method invoked when an <see cref="IEvent"/> is broadcast.
        /// </summary>
        /// <typeparam name="TEvent">The event type</typeparam>
        /// <param name="next">The next middleware into the chain</param>
        /// <param name="evt">The event broadcasted</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        Task OnEventAsync<TEvent>(Func<TEvent, CancellationToken, Task> next, TEvent evt, CancellationToken ct)
            where TEvent : class, IEvent;

        /// <summary>
        /// Method invoked when an <see cref="IQuery{TResult}"/> is fetched.
        /// </summary>
        /// <typeparam name="TQuery">The query type</typeparam>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="next">The next middleware into the chain</param>
        /// <param name="query">The query to fetch</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited for the result</returns>
        Task<TResult> OnQueryAsync<TQuery, TResult>(Func<TQuery, CancellationToken, Task<TResult>> next, TQuery query, CancellationToken ct)
            where TQuery : class, IQuery<TResult>;
    }
}
