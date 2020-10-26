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
using System.Threading;
using System.Threading.Tasks;

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// Sends commands of a given type into the handler
    /// </summary>
    /// <typeparam name="TCommand">The command type</typeparam>
    public class Sender<TCommand> : ISender<TCommand> where TCommand : class, ICommand
    {
        private readonly ICommandHandler<TCommand> _handler;
        private readonly List<IPipeline> _pipelines;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="pipelines"></param>
        public Sender(ICommandHandler<TCommand> handler, IEnumerable<IPipeline> pipelines)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            _pipelines = pipelines?.ToList() ?? throw new ArgumentNullException(nameof(pipelines));
        }

        /// <inheritdoc />
        public virtual Task SendAsync(TCommand cmd, CancellationToken ct)
        {
            if (cmd == null) throw new ArgumentNullException(nameof(cmd));

            if (_pipelines == null || _pipelines.Count == 0)
                return _handler.HandleAsync(cmd, ct);

            Func<TCommand, CancellationToken, Task> next = (command, c) => _handler.HandleAsync(command, c);

            for (var i = _pipelines.Count - 1; i >= 0; i--)
            {
                var pipeline = _pipelines[i];

                var old = next;
                next = (command, c) => pipeline.OnCommandAsync(old, command, c);
            }

            return next(cmd, ct);
        }
    }

    /// <summary>
    /// Sends commands of a given type into the handler
    /// </summary>
    /// <typeparam name="TCommand">The command type</typeparam>
    /// <typeparam name="TResult">The result type</typeparam>
    public class Sender<TCommand, TResult> : ISender<TCommand, TResult> where TCommand : class, ICommand<TResult>
    {
        private readonly ICommandHandler<TCommand, TResult> _handler;
        private readonly List<IPipeline> _pipelines;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="pipelines"></param>
        public Sender(ICommandHandler<TCommand, TResult> handler, IEnumerable<IPipeline> pipelines)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            _pipelines = pipelines?.ToList() ?? throw new ArgumentNullException(nameof(pipelines));
        }

        /// <inheritdoc />
        public virtual Task<TResult> SendAsync(TCommand cmd, CancellationToken ct)
        {
            if (cmd == null) throw new ArgumentNullException(nameof(cmd));

            if (_pipelines == null || _pipelines.Count == 0)
                return _handler.HandleAsync(cmd, ct);

            Func<TCommand, CancellationToken, Task<TResult>> next = (command, c) => _handler.HandleAsync(command, c);

            for (var i = _pipelines.Count - 1; i >= 0; i--)
            {
                var pipeline = _pipelines[i];

                var old = next;
                next = (command, c) => pipeline.OnCommandAsync(old, command, c);
            }

            return next(cmd, ct);
        }
    }
}
