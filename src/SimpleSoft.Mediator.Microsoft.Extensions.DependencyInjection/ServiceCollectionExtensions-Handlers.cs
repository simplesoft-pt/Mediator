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
using SimpleSoft.Mediator;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        #region ICommandHandler

        /// <summary>
        /// Registers the given type as an <see cref="ICommandHandler{TCommand}"/>.
        /// </summary>
        /// <typeparam name="TCommand">The command type</typeparam>
        /// <typeparam name="THandler">The handler type</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="lifetime">The handler lifetime</param>
        /// <returns>The service collection after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMediatorHandlerForCommand<TCommand, THandler>(
            this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where THandler : class, ICommandHandler<TCommand>
            where TCommand : ICommand
        {
            services.Add<ICommandHandler<TCommand>, THandler>(lifetime);

            return services;
        }

        /// <summary>
        /// Registers the given instance as an <see cref="ICommandHandler{TCommand}"/>.
        /// </summary>
        /// <typeparam name="TCommand">The command type</typeparam>
        /// <typeparam name="THandler">The handler type</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="instance">The instance to be used</param>
        /// <returns>The service collection after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMediatorHandlerForCommand<TCommand, THandler>(
            this IServiceCollection services, THandler instance)
            where THandler : class, ICommandHandler<TCommand>
            where TCommand : ICommand
        {
            services.AddSingleton<ICommandHandler<TCommand>>(instance);

            return services;
        }

        /// <summary>
        /// Registers the given function as a factory for <see cref="ICommandHandler{TCommand}"/>
        /// instances.
        /// </summary>
        /// <typeparam name="TCommand">The command type</typeparam>
        /// <typeparam name="THandler">The handler type</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="factory">The factory to be user</param>
        /// <param name="lifetime">The handler lifetime</param>
        /// <returns>The service collection after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMediatorHandlerForCommand<TCommand, THandler>(
            this IServiceCollection services, Func<IServiceProvider, THandler> factory, ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where THandler : class, ICommandHandler<TCommand>
            where TCommand : ICommand
        {
            services.Add<ICommandHandler<TCommand>>(factory, lifetime);

            return services;
        }

        /// <summary>
        /// Registers the given type as an <see cref="ICommandHandler{TCommand,TResult}"/>.
        /// </summary>
        /// <typeparam name="TCommand">The command type</typeparam>
        /// <typeparam name="THandler">The handler type</typeparam>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="lifetime">The handler lifetime</param>
        /// <returns>The service collection after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMediatorHandlerForCommand<TCommand, THandler, TResult>(
            this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where THandler : class, ICommandHandler<TCommand, TResult>
            where TCommand : ICommand<TResult>
        {
            services.Add<ICommandHandler<TCommand, TResult>, THandler>(lifetime);

            return services;
        }

        /// <summary>
        /// Registers the given instance as an <see cref="ICommandHandler{TCommand,TResult}"/>.
        /// </summary>
        /// <typeparam name="TCommand">The command type</typeparam>
        /// <typeparam name="THandler">The handler type</typeparam>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="instance">The instance to be used</param>
        /// <returns>The service collection after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMediatorHandlerForCommand<TCommand, THandler, TResult>(
            this IServiceCollection services, THandler instance)
            where THandler : class, ICommandHandler<TCommand, TResult>
            where TCommand : ICommand<TResult>
        {
            services.AddSingleton<ICommandHandler<TCommand, TResult>>(instance);

            return services;
        }

        /// <summary>
        /// Registers the given function as a factory for <see cref="ICommandHandler{TCommand,TResult}"/>
        /// instances.
        /// </summary>
        /// <typeparam name="TCommand">The command type</typeparam>
        /// <typeparam name="THandler">The handler type</typeparam>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="factory">The factory to be user</param>
        /// <param name="lifetime">The handler lifetime</param>
        /// <returns>The service collection after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMediatorHandlerForCommand<TCommand, THandler, TResult>(
            this IServiceCollection services, Func<IServiceProvider, THandler> factory, ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where THandler : class, ICommandHandler<TCommand, TResult>
            where TCommand : ICommand<TResult>
        {
            services.Add<ICommandHandler<TCommand, TResult>>(factory, lifetime);

            return services;
        }

        #endregion

        #region IEventHandler

        /// <summary>
        /// Registers the given type as an <see cref="IEventHandler{TEvent}"/>.
        /// </summary>
        /// <typeparam name="TEvent">The event type</typeparam>
        /// <typeparam name="THandler">The handler type</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="lifetime">The handler lifetime</param>
        /// <returns>The service collection after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMediatorHandlerForEvent<TEvent, THandler>(
            this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where THandler : class, IEventHandler<TEvent>
            where TEvent : IEvent
        {
            services.Add<IEventHandler<TEvent>, THandler>(lifetime);

            return services;
        }

        /// <summary>
        /// Registers the given instance as an <see cref="IEventHandler{TEvent}"/>.
        /// </summary>
        /// <typeparam name="TEvent">The event type</typeparam>
        /// <typeparam name="THandler">The handler type</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="instance">The instance to be used</param>
        /// <returns>The service collection after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMediatorHandlerForEvent<TEvent, THandler>(
            this IServiceCollection services, THandler instance)
            where THandler : class, IEventHandler<TEvent>
            where TEvent : IEvent
        {
            services.AddSingleton<IEventHandler<TEvent>>(instance);

            return services;
        }

        /// <summary>
        /// Registers the given function as a factory for <see cref="IEventHandler{TEvent}"/>
        /// instances.
        /// </summary>
        /// <typeparam name="TEvent">The event type</typeparam>
        /// <typeparam name="THandler">The handler type</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="factory">The factory to be user</param>
        /// <param name="lifetime">The handler lifetime</param>
        /// <returns>The service collection after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMediatorHandlerForEvent<TEvent, THandler>(
            this IServiceCollection services, Func<IServiceProvider, THandler> factory, ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where THandler : class, IEventHandler<TEvent>
            where TEvent : IEvent
        {
            services.Add<IEventHandler<TEvent>>(factory, lifetime);

            return services;
        }

        #endregion

        #region IQueryHandler

        /// <summary>
        /// Registers the given type as an <see cref="IQueryHandler{TQuery,TResult}"/>.
        /// </summary>
        /// <typeparam name="TQuery">The query type</typeparam>
        /// <typeparam name="THandler">The handler type</typeparam>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="lifetime">The handler lifetime</param>
        /// <returns>The service collection after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMediatorHandlerForQuery<TQuery, THandler, TResult>(
            this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where THandler : class, IQueryHandler<TQuery, TResult>
            where TQuery : IQuery<TResult>
        {
            services.Add<IQueryHandler<TQuery, TResult>, THandler>(lifetime);

            return services;
        }

        /// <summary>
        /// Registers the given instance as an <see cref="IQueryHandler{TQuery,TResult}"/>.
        /// </summary>
        /// <typeparam name="TQuery">The query type</typeparam>
        /// <typeparam name="THandler">The handler type</typeparam>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="instance">The instance to be used</param>
        /// <returns>The service collection after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMediatorHandlerForQuery<TQuery, THandler, TResult>(
            this IServiceCollection services, THandler instance)
            where THandler : class, IQueryHandler<TQuery, TResult>
            where TQuery : IQuery<TResult>
        {
            services.AddSingleton<IQueryHandler<TQuery, TResult>>(instance);

            return services;
        }

        /// <summary>
        /// Registers the given function as a factory for <see cref="IQueryHandler{TQuery,TResult}"/>
        /// instances.
        /// </summary>
        /// <typeparam name="TQuery">The command type</typeparam>
        /// <typeparam name="THandler">The handler type</typeparam>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="factory">The factory to be user</param>
        /// <param name="lifetime">The handler lifetime</param>
        /// <returns>The service collection after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMediatorHandlerForQuery<TQuery, THandler, TResult>(
            this IServiceCollection services, Func<IServiceProvider, THandler> factory, ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where THandler : class, IQueryHandler<TQuery, TResult>
            where TQuery : IQuery<TResult>
        {
            services.Add<IQueryHandler<TQuery, TResult>>(factory, lifetime);

            return services;
        }

        #endregion
    }
}
