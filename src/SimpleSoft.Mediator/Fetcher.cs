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
    /// Fetches data of a given query type from the handler
    /// </summary>
    /// <typeparam name="TQuery">The query type</typeparam>
    /// <typeparam name="TResult">The result type</typeparam>
    public class Fetcher<TQuery, TResult> : IFetcher<TQuery, TResult> where TQuery : class, IQuery<TResult>
    {
        private readonly IQueryHandler<TQuery, TResult> _handler;
        private readonly List<IPipeline> _pipelines;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="pipelines"></param>
        public Fetcher(IQueryHandler<TQuery, TResult> handler, IEnumerable<IPipeline> pipelines)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            _pipelines = (pipelines ?? throw new ArgumentNullException(nameof(pipelines))).ToList();
        }

        /// <inheritdoc />
        public virtual Task<TResult> FetchAsync(TQuery query, CancellationToken ct)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            if (_pipelines == null || _pipelines.Count == 0)
                return _handler.HandleAsync(query, ct);

            Func<TQuery, CancellationToken, Task<TResult>> next = (qry, c) => _handler.HandleAsync(qry, c);

            for (var i = _pipelines.Count - 1; i >= 0; i--)
            {
                var pipeline = _pipelines[i];

                var old = next;
                next = (qry, c) => pipeline.OnQueryAsync(old, qry, c);
            }

            return next(query, ct);
        }
    }
}