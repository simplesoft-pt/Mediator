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

namespace SimpleSoft.Mediator.Pipeline
{
    /// <summary>
    /// Filter executed when the handling of commands or events fails.
    /// </summary>
    public interface IHandlingFailedFilter
    {
        /// <summary>
        /// Executed when the handling of an <see cref="ICommand"/> fails.
        /// </summary>
        /// <typeparam name="TCommand">The command type</typeparam>
        /// <param name="cmd">The command to be handled</param>
        /// <param name="exception">The exception thrown</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        Task OnFailedCommandAsync<TCommand>(TCommand cmd, Exception exception, CancellationToken ct)
            where TCommand : ICommand;

        /// <summary>
        /// Executed when the handling of an <see cref="ICommand{TResult}"/> fails.
        /// </summary>
        /// <typeparam name="TCommand">The command type</typeparam>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="cmd">The command to be handled</param>
        /// <param name="exception">The exception thrown</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        Task OnFailedCommandAsync<TCommand, TResult>(TCommand cmd, Exception exception, CancellationToken ct)
            where TCommand : ICommand<TResult>;

        /// <summary>
        /// Executed when the handling of an <see cref="IEvent"/> fails.
        /// </summary>
        /// <typeparam name="TEvent">The event type</typeparam>
        /// <param name="evt">The event to be handled</param>
        /// <param name="exception">The exception thrown</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited</returns>
        Task OnFailedEventAsync<TEvent>(TEvent evt, Exception exception, CancellationToken ct)
            where TEvent : IEvent;
    }
}