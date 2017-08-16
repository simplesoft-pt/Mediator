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
using SimpleSoft.Mediator.Internal;

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// Builds handlers using delegate functions
    /// </summary>
    public static class DelegateHandler
    {
        /// <summary>
        /// Builds a command handler that wraps the given delegate.
        /// </summary>
        /// <typeparam name="TCommand">The command type</typeparam>
        /// <param name="handler">The delegate to wrap</param>
        /// <returns>The command handler</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static ICommandHandler<TCommand> Command<TCommand>(Func<TCommand, CancellationToken, Task> handler)
            where TCommand : ICommand
        {
            return new DelegateCommandHandler<TCommand>(handler);
        }

        /// <summary>
        /// Builds a command handler that wraps the given delegate.
        /// </summary>
        /// <typeparam name="TCommand">The command type</typeparam>
        /// <typeparam name="TResult">The command result type</typeparam>
        /// <param name="handler">The delegate to wrap</param>
        /// <returns>The command handler</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static ICommandHandler<TCommand, TResult> Command<TCommand, TResult>(Func<TCommand, CancellationToken, Task<TResult>> handler)
            where TCommand : ICommand<TResult>
        {
            return new DelegateCommandHandler<TCommand, TResult>(handler);
        }

        /// <summary>
        /// Builds an event handler that wraps the given delegate.
        /// </summary>
        /// <typeparam name="TEvent">The event type</typeparam>
        /// <param name="handler">The delegate to wrap</param>
        /// <returns>The event handler</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEventHandler<TEvent> Event<TEvent>(Func<TEvent, CancellationToken, Task> handler)
            where TEvent : IEvent
        {
            return new DelegateEventHandler<TEvent>(handler);
        }
    }
}
