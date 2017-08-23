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

using System.Threading;
using System.Threading.Tasks;

namespace SimpleSoft.Mediator.Pipeline
{
    /// <summary>
    /// Handling middleware that can be used to intercept commands events and queries
    /// </summary>
    public abstract class Middleware : IMiddleware
    {
        /// <inheritdoc />
        public virtual async Task OnCommandAsync<TCommand>(CommandMiddlewareDelegate<TCommand> next, TCommand cmd, CancellationToken ct) 
            where TCommand : ICommand
        {
            await next(cmd, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public virtual async Task<TResult> OnCommandAsync<TCommand, TResult>(CommandMiddlewareDelegate<TCommand, TResult> next, TCommand cmd, CancellationToken ct)
            where TCommand : ICommand<TResult>
        {
            return await next(cmd, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public virtual async Task OnEventAsync<TEvent>(EventMiddlewareDelegate<TEvent> next, TEvent evt, CancellationToken ct) 
            where TEvent : IEvent
        {
            await next(evt, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public virtual async Task<TResult> OnQueryAsync<TQuery, TResult>(QueryMiddlewareDelegate<TQuery, TResult> next, TQuery query, CancellationToken ct)
            where TQuery : IQuery<TResult>
        {
            return await next(query, ct).ConfigureAwait(false);
        }
    }
}