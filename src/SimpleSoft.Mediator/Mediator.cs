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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// Mediator to publish commands, broadcast events and fetch queries
    /// </summary>
    public class Mediator : IMediator
    {
        private static readonly List<IPipeline> EmptyPipelines;
        private static readonly ConcurrentDictionary<Type, Func<Mediator, object, CancellationToken, object>> ExpressionCache;
        private static readonly Task CompletedTask;
        private static readonly MethodInfo MethodInternalSendWithResult;
        private static readonly MethodInfo MethodInternalSend;
        private static readonly MethodInfo MethodInternalBroadcast;
        private static readonly MethodInfo MethodInternalFetch;

        static Mediator()
        {
            EmptyPipelines = new List<IPipeline>(0);
            ExpressionCache = new ConcurrentDictionary<Type, Func<Mediator, object, CancellationToken, object>>();
#if NET40
            var tcs = new TaskCompletionSource<bool>();
            tcs.SetResult(true);
            CompletedTask = tcs.Task;
#else
            CompletedTask = Task.FromResult(true);
#endif

#if NETSTANDARD1_1
            var methods = typeof(Mediator).GetTypeInfo().DeclaredMethods;
#else
            var methods = typeof(Mediator).GetMethods();
#endif

            MethodInfo internalSendWithResult = null;
            MethodInfo internalSend = null;
            MethodInfo internalBroadcast = null;
            MethodInfo internalFetch = null;
            
            foreach (var method in methods)
            {
                switch (method.Name)
                {
                    case nameof(InternalSendAsync):
                        switch (method.GetGenericArguments().Length)
                        {
                            case 2:
                                internalSendWithResult = method;
                                break;
                            case 1:
                                internalSend = method;
                                break;
                        }
                        break;
                    case nameof(InternalBroadcastAsync):
                        internalBroadcast = method;
                        break;
                    case nameof(InternalFetchAsync):
                        internalFetch = method;
                        break;
                }
            }

            MethodInternalSendWithResult = internalSendWithResult ?? throw new InvalidOperationException("Method 'InternalSendAsync<TCommand, TResult>' not found");
            MethodInternalSend = internalSend ?? throw new InvalidOperationException("Method 'InternalSendAsync<TCommand>' not found");
            MethodInternalBroadcast = internalBroadcast ?? throw new InvalidOperationException("Method 'InternalBroadcastAsync<TEvent>' not found");
            MethodInternalFetch = internalFetch ?? throw new InvalidOperationException("Method 'InternalFetchAsync<TQuery, TResult>' not found");
        }

        private readonly IMediatorServiceProvider _serviceProvider;
        private readonly List<IPipeline> _reversedPipelines;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="serviceProvider">The handler factory</param>
        /// <param name="pipelines">The mediator pipeline collection</param>
        /// <exception cref="ArgumentNullException"></exception>
        public Mediator(IMediatorServiceProvider serviceProvider, IEnumerable<IPipeline> pipelines = null)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            if (pipelines == null)
                _reversedPipelines = EmptyPipelines;
            else
            {
                _reversedPipelines = new List<IPipeline>(pipelines);
                _reversedPipelines.Reverse();
            }
        }

        /// <inheritdoc />
        public Task SendAsync(ICommand cmd, CancellationToken ct)
        {
            if (cmd == null) throw new ArgumentNullException(nameof(cmd));

            var caller = ExpressionCache.GetOrAdd(cmd.GetType(), commandType =>
            {
                var mediatorParameter = Expression.Parameter(typeof(Mediator));
                var commandParameter = Expression.Parameter(typeof(object));
                var cancellationTokenParameter = Expression.Parameter(typeof(CancellationToken));

                return Expression.Lambda<Func<Mediator, object, CancellationToken, object>>(
                    Expression.Call(
                        mediatorParameter,
                        MethodInternalSend.MakeGenericMethod(commandType),
                        Expression.TypeAs(commandParameter, commandType),
                        cancellationTokenParameter
                    ),
                    mediatorParameter,
                    commandParameter,
                    cancellationTokenParameter
                ).Compile();
            });

            return (Task) caller(this, cmd, ct);
        }

        /// <inheritdoc />
        public Task<TResult> SendAsync<TResult>(ICommand<TResult> cmd, CancellationToken ct)
        {
            if (cmd == null) throw new ArgumentNullException(nameof(cmd));

            var caller = ExpressionCache.GetOrAdd(cmd.GetType(), commandType =>
            {
#if NETSTANDARD1_1
                var interfaces = commandType.GetTypeInfo().ImplementedInterfaces;
#else
                var interfaces = commandType.GetInterfaces();
#endif
                var returnType = interfaces
                    .Single(i =>
#if NETSTANDARD1_1
                        i.GetTypeInfo().IsGenericType &&
#else
                        i.IsGenericType &&
#endif
                        i.GetGenericTypeDefinition() == typeof(ICommand<>)
#if NET40
                    ).GetGenericArguments()[0];
#else
                    ).GenericTypeArguments[0];
#endif

                var mediatorParameter = Expression.Parameter(typeof(Mediator));
                var commandParameter = Expression.Parameter(typeof(object));
                var cancellationTokenParameter = Expression.Parameter(typeof(CancellationToken));

                return Expression.Lambda<Func<Mediator, object, CancellationToken, object>>(
                    Expression.Call(
                        mediatorParameter,
                        MethodInternalSendWithResult.MakeGenericMethod(commandType, returnType),
                        Expression.TypeAs(commandParameter, commandType),
                        cancellationTokenParameter
                    ),
                    mediatorParameter,
                    commandParameter,
                    cancellationTokenParameter
                ).Compile();
            });

            return (Task<TResult>) caller(this, cmd, ct);
        }

        /// <inheritdoc />
        public Task BroadcastAsync(IEvent evt, CancellationToken ct)
        {
            if (evt == null) throw new ArgumentNullException(nameof(evt));

            var caller = ExpressionCache.GetOrAdd(evt.GetType(), eventType =>
            {
                var mediatorParameter = Expression.Parameter(typeof(Mediator));
                var eventParameter = Expression.Parameter(typeof(object));
                var cancellationTokenParameter = Expression.Parameter(typeof(CancellationToken));

                return Expression.Lambda<Func<Mediator, object, CancellationToken, object>>(
                    Expression.Call(
                        mediatorParameter,
                        MethodInternalBroadcast.MakeGenericMethod(eventType),
                        Expression.TypeAs(eventParameter, eventType),
                        cancellationTokenParameter
                    ),
                    mediatorParameter,
                    eventParameter,
                    cancellationTokenParameter
                ).Compile();
            });

            return (Task) caller(this, evt, ct);
        }

        /// <inheritdoc />
        public Task<TResult> FetchAsync<TResult>(IQuery<TResult> query, CancellationToken ct)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var caller = ExpressionCache.GetOrAdd(query.GetType(), queryType =>
            {
#if NETSTANDARD1_1
                var interfaces = queryType.GetTypeInfo().ImplementedInterfaces;
#else
                var interfaces = queryType.GetInterfaces();
#endif
                var returnType = interfaces
                    .Single(i =>
#if NETSTANDARD1_1
                        i.GetTypeInfo().IsGenericType &&
#else
                        i.IsGenericType &&
#endif
                        i.GetGenericTypeDefinition() == typeof(IQuery<>)
#if NET40
                    ).GetGenericArguments()[0];
#else
                    ).GenericTypeArguments[0];
#endif

                var mediatorParameter = Expression.Parameter(typeof(Mediator));
                var queryParameter = Expression.Parameter(typeof(object));
                var cancellationTokenParameter = Expression.Parameter(typeof(CancellationToken));

                return Expression.Lambda<Func<Mediator, object, CancellationToken, object>>(
                    Expression.Call(
                        mediatorParameter,
                        MethodInternalFetch.MakeGenericMethod(queryType, returnType),
                        Expression.TypeAs(queryParameter, queryType),
                        cancellationTokenParameter
                    ),
                    mediatorParameter,
                    queryParameter,
                    cancellationTokenParameter
                ).Compile();
            });

            return (Task<TResult>) caller(this, query, ct);
        }

        #region Typed Implementations

        /// <summary>
        /// Typed implementation for <see cref="SendAsync"/>.
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="cmd"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public Task InternalSendAsync<TCommand>(TCommand cmd, CancellationToken ct)
            where TCommand : class, ICommand
        {
            if (cmd == null) throw new ArgumentNullException(nameof(cmd));

            Func<TCommand, CancellationToken, Task> next = (c, cancellationToken) =>
            {
                var handler = _serviceProvider.BuildService<ICommandHandler<TCommand>>();
                if (handler == null)
                    throw CommandHandlerNotFoundException.Build(c);
                return handler.HandleAsync(c, cancellationToken);
            };

            foreach (var middleware in _reversedPipelines)
            {
                var old = next;
                next = (c, cancellationToken) => middleware.OnCommandAsync(old, c, cancellationToken);
            }

            return next(cmd, ct);
        }

        /// <summary>
        /// Typed implementation for <see cref="SendAsync{TResult}"/>.
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="cmd"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public Task<TResult> InternalSendAsync<TCommand, TResult>(TCommand cmd, CancellationToken ct)
            where TCommand : class, ICommand<TResult>
        {
            if (cmd == null) throw new ArgumentNullException(nameof(cmd));

            Func<TCommand, CancellationToken, Task<TResult>> next = (c, cancellationToken) =>
            {
                var handler = _serviceProvider.BuildService<ICommandHandler<TCommand, TResult>>();
                if (handler == null)
                    throw CommandHandlerNotFoundException.Build<TCommand, TResult>(c);
                return handler.HandleAsync(c, cancellationToken);
            };

            foreach (var middleware in _reversedPipelines)
            {
                var old = next;
                next = (c, cancellationToken) => middleware.OnCommandAsync(old, c, cancellationToken);
            }

            return next(cmd, ct);
        }

        /// <summary>
        /// Typed implementation for <see cref="BroadcastAsync"/>
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="evt"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public Task InternalBroadcastAsync<TEvent>(TEvent evt, CancellationToken ct)
            where TEvent : class, IEvent
        {
            if (evt == null) throw new ArgumentNullException(nameof(evt));

            Func<TEvent, CancellationToken, Task> next = (e, cancellationToken) =>
            {
                var handlers = _serviceProvider
                    .BuildServices<IEventHandler<TEvent>>()
                    .Select(handler => handler.HandleAsync(e, cancellationToken))
                    .ToArray();
                if (handlers.Length == 0)
                    return CompletedTask;

                var tcs = new TaskCompletionSource<bool>();
                Task.Factory.ContinueWhenAll(handlers, tasks =>
                {
                    List<Exception> exceptions = null;
                    foreach (var t in tasks)
                    {
                        if (t.Exception == null)
                            continue;

                        exceptions ??= new List<Exception>(tasks.Length);
                        exceptions.Add(t.Exception.InnerException);
                    }

                    if (exceptions == null)
                        tcs.SetResult(true);
                    else
                        tcs.SetException(new AggregateException(exceptions));
                }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Current);

                return tcs.Task;
            };

            foreach (var middleware in _reversedPipelines)
            {
                var old = next;
                next = (e, cancellationToken) => middleware.OnEventAsync(old, e, cancellationToken);
            }

            return next(evt, ct);
        }

        /// <summary>
        /// Typed implementation for <see cref="FetchAsync{TResult}"/>
        /// </summary>
        /// <typeparam name="TQuery"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public Task<TResult> InternalFetchAsync<TQuery, TResult>(TQuery query, CancellationToken ct)
            where TQuery : class, IQuery<TResult>
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            Func<TQuery, CancellationToken, Task<TResult>> next = (q, cancellationToken) =>
            {
                var handler = _serviceProvider.BuildService<IQueryHandler<TQuery, TResult>>();
                if (handler == null)
                    throw QueryHandlerNotFoundException.Build<TQuery, TResult>(q);
                return handler.HandleAsync(q, cancellationToken);
            };

            foreach (var middleware in _reversedPipelines)
            {
                var old = next;
                next = (q, cancellationToken) => middleware.OnQueryAsync(old, q, cancellationToken);
            }

            return next(query, ct);
        }

#endregion
    }
}
