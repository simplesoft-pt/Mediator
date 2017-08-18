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
using TaskExtensions = SimpleSoft.Mediator.Internal.TaskExtensions;

namespace SimpleSoft.Mediator.Pipeline
{
    /// <summary>
    /// Filter that is run on every handling event
    /// </summary>
    public abstract class HandlingFilter : IHandlingFilter
    {
        /// <inheritdoc />
        public virtual Task OnExecutingCommandAsync<TCommand>(TCommand cmd, CancellationToken ct) where TCommand : ICommand
        {
            return TaskExtensions.CompletedTask;
        }

        /// <inheritdoc />
        public virtual Task OnExecutingCommandAsync<TCommand, TResult>(TCommand cmd, CancellationToken ct) where TCommand : ICommand<TResult>
        {
            return TaskExtensions.CompletedTask;
        }

        /// <inheritdoc />
        public virtual Task OnExecutingEventAsync<TEvent>(TEvent evt, CancellationToken ct) where TEvent : IEvent
        {
            return TaskExtensions.CompletedTask;
        }
        
        /// <inheritdoc />
        public virtual Task OnExecutedCommandAsync<TCommand>(TCommand cmd, CancellationToken ct) where TCommand : ICommand
        {
            return TaskExtensions.CompletedTask;
        }

        /// <inheritdoc />
        public virtual Task OnExecutedCommandAsync<TCommand, TResult>(TCommand cmd, TResult result, CancellationToken ct) where TCommand : ICommand<TResult>
        {
            return TaskExtensions.CompletedTask;
        }

        /// <inheritdoc />
        public virtual Task OnExecutedEventAsync<TEvent>(TEvent evt, CancellationToken ct) where TEvent : IEvent
        {
            return TaskExtensions.CompletedTask;
        }

        /// <inheritdoc />
        public virtual Task OnFailedCommandAsync<TCommand>(TCommand cmd, Exception exception, CancellationToken ct) where TCommand : ICommand
        {
            return TaskExtensions.CompletedTask;
        }

        /// <inheritdoc />
        public virtual Task OnFailedCommandAsync<TCommand, TResult>(TCommand cmd, Exception exception, CancellationToken ct) where TCommand : ICommand<TResult>
        {
            return TaskExtensions.CompletedTask;
        }

        /// <inheritdoc />
        public virtual Task OnFailedEventAsync<TEvent>(TEvent evt, Exception exception, CancellationToken ct) where TEvent : IEvent
        {
            return TaskExtensions.CompletedTask;
        }
    }
}