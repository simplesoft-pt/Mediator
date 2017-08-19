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

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// Exception thrown when the handler for a given <see cref="IQuery{TResult}"/>
    /// can't be found.
    /// </summary>
    public class QueryHandlerNotFoundException : InvalidOperationException
    {
        private const string DefaultMessageFormat = "The query '{0}' does not have any handler";

        private QueryHandlerNotFoundException(Type queryType, object queryData)
            : base(string.Format(DefaultMessageFormat, queryType.Name))
        {
            QueryType = queryType;
            QueryData = queryData;
        }

        /// <summary>
        /// The command type
        /// </summary>
        public Type QueryType { get; }

        /// <summary>
        /// The command name
        /// </summary>
        public string QueryName => QueryType.Name;

        /// <summary>
        /// The command that caused this exception
        /// </summary>
        public object QueryData { get; }

        /// <summary>
        /// Returns the <see cref="QueryData"/> instance as the
        /// given <see cref="IQuery{TResult}"/>.
        /// </summary>
        /// <typeparam name="TQuery">The query type</typeparam>
        /// <typeparam name="TResult">The query result type</typeparam>
        /// <returns>The query</returns>
        public TQuery Query<TQuery, TResult>() where TQuery : IQuery<TResult>
        {
            return (TQuery)QueryData;
        }
        

        /// <summary>
        /// Builds a new exception.
        /// </summary>
        /// <typeparam name="TQuery">The query type</typeparam>
        /// <typeparam name="TResult">The query result type</typeparam>
        /// <returns>The exception instance</returns>
        public static QueryHandlerNotFoundException Build<TQuery, TResult>(TQuery cmd)
            where TQuery : IQuery<TResult>
        {
            if (cmd == null) throw new ArgumentNullException(nameof(cmd));

            return new QueryHandlerNotFoundException(typeof(TQuery), cmd);
        }
    }
}