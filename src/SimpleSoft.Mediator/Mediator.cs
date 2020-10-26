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
        private static readonly ConcurrentDictionary<Type, Func<IMediatorServiceProvider, object, CancellationToken, object>> ExpressionCache;
        private static readonly MethodInfo MethodServiceProviderBuildService;

        static Mediator()
        {
            ExpressionCache = new ConcurrentDictionary<Type, Func<IMediatorServiceProvider, object, CancellationToken, object>>();
            
            MethodServiceProviderBuildService = GetMethods(typeof(IMediatorServiceProvider))
                .Single(m => m.Name == nameof(IMediatorServiceProvider.BuildService));
        }

        private readonly IMediatorServiceProvider _serviceProvider;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="serviceProvider">The handler factory</param>
        /// <exception cref="ArgumentNullException"></exception>
        public Mediator(IMediatorServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        /// <inheritdoc />
        public Task SendAsync(ICommand cmd, CancellationToken ct)
        {
            if (cmd == null) throw new ArgumentNullException(nameof(cmd));

            //Func<IMediatorServiceProvider, object, CancellationToken, object> caller = (provider, obj, cancellationToken) =>
            //{
            //    return provider
            //        .BuildService<ISender<TCommand>>()
            //        .SendAsync((TCommand) obj, cancellationToken);
            //};

            var caller = ExpressionCache.GetOrAdd(cmd.GetType(), commandType =>
            {
                var senderType = typeof(ISender<>).MakeGenericType(commandType);

                var mediatorServiceProviderParameter = Expression.Parameter(typeof(IMediatorServiceProvider));
                var commandParameter = Expression.Parameter(typeof(object));
                var cancellationTokenParameter = Expression.Parameter(typeof(CancellationToken));

                return Expression.Lambda<Func<IMediatorServiceProvider, object, CancellationToken, object>>(
                    Expression.Call(
                        Expression.Call(
                            mediatorServiceProviderParameter,
                            MethodServiceProviderBuildService.MakeGenericMethod(senderType)
                        ),
                        GetMethods(senderType).Single(m => m.Name == "SendAsync"),
                        Expression.TypeAs(commandParameter, commandType),
                        cancellationTokenParameter
                    ),
                    mediatorServiceProviderParameter,
                    commandParameter,
                    cancellationTokenParameter
                ).Compile();
            });

            return (Task) caller(_serviceProvider, cmd, ct);
        }

        /// <inheritdoc />
        public Task<TResult> SendAsync<TResult>(ICommand<TResult> cmd, CancellationToken ct)
        {
            if (cmd == null) throw new ArgumentNullException(nameof(cmd));

            //Func<IMediatorServiceProvider, object, CancellationToken, object> caller = (provider, obj, cancellationToken) =>
            //{
            //    return provider
            //        .BuildService<ISender<TCommand<TResult>, TResult>>()
            //        .SendAsync((TCommand<TResult>) obj, cancellationToken);
            //};

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

                var senderType = typeof(ISender<,>).MakeGenericType(commandType, returnType);

                var mediatorServiceProviderParameter = Expression.Parameter(typeof(IMediatorServiceProvider));
                var commandParameter = Expression.Parameter(typeof(object));
                var cancellationTokenParameter = Expression.Parameter(typeof(CancellationToken));

                return Expression.Lambda<Func<IMediatorServiceProvider, object, CancellationToken, object>>(
                    Expression.Call(
                        Expression.Call(
                            mediatorServiceProviderParameter,
                            MethodServiceProviderBuildService.MakeGenericMethod(senderType)
                        ),
                        GetMethods(senderType).Single(m => m.Name == "SendAsync"),
                        Expression.TypeAs(commandParameter, commandType),
                        cancellationTokenParameter
                    ),
                    mediatorServiceProviderParameter,
                    commandParameter,
                    cancellationTokenParameter
                ).Compile();
            });

            return (Task<TResult>) caller(_serviceProvider, cmd, ct);
        }

        /// <inheritdoc />
        public Task BroadcastAsync(IEvent evt, CancellationToken ct)
        {
            if (evt == null) throw new ArgumentNullException(nameof(evt));

            //Func<IMediatorServiceProvider, object, CancellationToken, object> caller = (provider, obj, cancellationToken) =>
            //{
            //    return provider
            //        .BuildService<IBroadcaster<TEvent>>()
            //        .BroadcastAsync((TEvent) obj, cancellationToken);
            //};

            var caller = ExpressionCache.GetOrAdd(evt.GetType(), eventType =>
            {
                var broadcasterType = typeof(IBroadcaster<>).MakeGenericType(eventType);

                var mediatorServiceProviderParameter = Expression.Parameter(typeof(IMediatorServiceProvider));
                var eventParameter = Expression.Parameter(typeof(object));
                var cancellationTokenParameter = Expression.Parameter(typeof(CancellationToken));

                return Expression.Lambda<Func<IMediatorServiceProvider, object, CancellationToken, object>>(
                    Expression.Call(
                        Expression.Call(
                            mediatorServiceProviderParameter,
                            MethodServiceProviderBuildService.MakeGenericMethod(broadcasterType)
                        ),
                        GetMethods(broadcasterType).Single(m => m.Name == "BroadcastAsync"),
                        Expression.TypeAs(eventParameter, eventType),
                        cancellationTokenParameter
                    ),
                    mediatorServiceProviderParameter,
                    eventParameter,
                    cancellationTokenParameter
                ).Compile();
            });

            return (Task) caller(_serviceProvider, evt, ct);
        }

        /// <inheritdoc />
        public Task<TResult> FetchAsync<TResult>(IQuery<TResult> query, CancellationToken ct)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            //Func<IMediatorServiceProvider, object, CancellationToken, object> caller = (provider, obj, cancellationToken) =>
            //{
            //    return provider
            //        .BuildService<IFetcher<TQuery<TResult>, TResult>>()
            //        .FetchAsync((TQuery<TResult>) obj, cancellationToken);
            //};

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

                var fetcherType = typeof(IFetcher<,>).MakeGenericType(queryType, returnType);

                var mediatorServiceProviderParameter = Expression.Parameter(typeof(IMediatorServiceProvider));
                var queryParameter = Expression.Parameter(typeof(object));
                var cancellationTokenParameter = Expression.Parameter(typeof(CancellationToken));

                return Expression.Lambda<Func<IMediatorServiceProvider, object, CancellationToken, object>>(
                    Expression.Call(
                        Expression.Call(
                            mediatorServiceProviderParameter,
                            MethodServiceProviderBuildService.MakeGenericMethod(fetcherType)
                        ),
                        GetMethods(fetcherType).Single(m => m.Name == "FetchAsync"),
                        Expression.TypeAs(queryParameter, queryType),
                        cancellationTokenParameter
                    ),
                    mediatorServiceProviderParameter,
                    queryParameter,
                    cancellationTokenParameter
                ).Compile();
            });

            return (Task<TResult>) caller(_serviceProvider, query, ct);
        }

        private static IEnumerable<MethodInfo> GetMethods(Type type)
        {
#if NETSTANDARD1_1
            return type.GetTypeInfo().DeclaredMethods;
#else
            return type.GetMethods();
#endif
        }
    }
}
