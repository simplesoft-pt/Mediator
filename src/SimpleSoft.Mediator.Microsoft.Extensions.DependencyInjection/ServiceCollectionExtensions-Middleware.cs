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
using SimpleSoft.Mediator.Middleware;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        #region ICommandMiddleware

        /// <summary>
        /// Registers the given type as a <see cref="ICommandMiddleware"/>.
        /// </summary>
        /// <typeparam name="T">The middleware implementation type</typeparam>
        /// <param name="services">The services collection</param>
        /// <param name="lifetime">The middleware lifetime</param>
        /// <returns>The service collection after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMediatorMiddlewareForCommands<T>(
            this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where T : class, ICommandMiddleware
        {
            services.Add<ICommandMiddleware, T>(lifetime);

            return services;
        }

        /// <summary>
        /// Registers the given instance as a <see cref="ICommandMiddleware"/>.
        /// </summary>
        /// <typeparam name="T">The middleware implementation type</typeparam>
        /// <param name="services">The services collection</param>
        /// <param name="instance">The instance to register</param>
        /// <returns>The service collection after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMediatorMiddlewareForCommands<T>(
            this IServiceCollection services, T instance)
            where T : class, ICommandMiddleware
        {
            services.AddSingleton<ICommandMiddleware>(instance);

            return services;
        }

        /// <summary>
        /// Registers the given function as a factory for <see cref="ICommandMiddleware"/>.
        /// </summary>
        /// <typeparam name="T">The middleware implementation type</typeparam>
        /// <param name="services">The services collection</param>
        /// <param name="factory">The factory method</param>
        /// <param name="lifetime">The middleware lifetime</param>
        /// <returns>The service collection after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMediatorMiddlewareForCommands<T>(
            this IServiceCollection services, Func<IServiceProvider, T> factory,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where T : class, ICommandMiddleware
        {
            services.Add<ICommandMiddleware>(factory, lifetime);

            return services;
        }

        #endregion

        #region IEventMiddleware

        /// <summary>
        /// Registers the given type as a <see cref="IEventMiddleware"/>.
        /// </summary>
        /// <typeparam name="T">The middleware implementation type</typeparam>
        /// <param name="services">The services collection</param>
        /// <param name="lifetime">The middleware lifetime</param>
        /// <returns>The service collection after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMediatorMiddlewareForEvents<T>(
            this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where T : class, IEventMiddleware
        {
            services.Add<IEventMiddleware, T>(lifetime);

            return services;
        }

        /// <summary>
        /// Registers the given instance as a <see cref="IEventMiddleware"/>.
        /// </summary>
        /// <typeparam name="T">The middleware implementation type</typeparam>
        /// <param name="services">The services collection</param>
        /// <param name="instance">The instance to register</param>
        /// <returns>The service collection after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMediatorMiddlewareForEvents<T>(
            this IServiceCollection services, T instance)
            where T : class, IEventMiddleware
        {
            services.AddSingleton<IEventMiddleware>(instance);

            return services;
        }

        /// <summary>
        /// Registers the given function as a factory for <see cref="IEventMiddleware"/>.
        /// </summary>
        /// <typeparam name="T">The middleware implementation type</typeparam>
        /// <param name="services">The services collection</param>
        /// <param name="factory">The factory method</param>
        /// <param name="lifetime">The middleware lifetime</param>
        /// <returns>The service collection after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMediatorMiddlewareForEvents<T>(
            this IServiceCollection services, Func<IServiceProvider, T> factory,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where T : class, IEventMiddleware
        {
            services.Add<IEventMiddleware>(factory, lifetime);

            return services;
        }

        #endregion

        #region IQueryMiddleware

        /// <summary>
        /// Registers the given type as a <see cref="IQueryMiddleware"/>.
        /// </summary>
        /// <typeparam name="T">The middleware implementation type</typeparam>
        /// <param name="services">The services collection</param>
        /// <param name="lifetime">The middleware lifetime</param>
        /// <returns>The service collection after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMediatorMiddlewareForQueries<T>(
            this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where T : class, IQueryMiddleware
        {
            services.Add<IQueryMiddleware, T>(lifetime);

            return services;
        }

        /// <summary>
        /// Registers the given instance as a <see cref="IQueryMiddleware"/>.
        /// </summary>
        /// <typeparam name="T">The middleware implementation type</typeparam>
        /// <param name="services">The services collection</param>
        /// <param name="instance">The instance to register</param>
        /// <returns>The service collection after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMediatorMiddlewareForQueries<T>(
            this IServiceCollection services, T instance)
            where T : class, IQueryMiddleware
        {
            services.AddSingleton<IQueryMiddleware>(instance);

            return services;
        }

        /// <summary>
        /// Registers the given function as a factory for <see cref="IQueryMiddleware"/>.
        /// </summary>
        /// <typeparam name="T">The middleware implementation type</typeparam>
        /// <param name="services">The services collection</param>
        /// <param name="factory">The factory method</param>
        /// <param name="lifetime">The middleware lifetime</param>
        /// <returns>The service collection after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMediatorMiddlewareForQueries<T>(
            this IServiceCollection services, Func<IServiceProvider, T> factory,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where T : class, IQueryMiddleware
        {
            services.Add<IQueryMiddleware>(factory, lifetime);

            return services;
        }

        #endregion

        #region IMiddleware

        /// <summary>
        /// Registers the given type as a <see cref="ICommandMiddleware"/>,
        /// <see cref="IEventMiddleware"/> and <see cref="IQueryMiddleware"/>.
        /// </summary>
        /// <typeparam name="T">The middleware implementation type</typeparam>
        /// <param name="services">The services collection</param>
        /// <param name="lifetime">The middleware lifetime</param>
        /// <returns>The service collection after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMediatorMiddleware<T>(
            this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where T : class, IMiddleware
        {
            services.Add<T, T>(lifetime);
            services.Add<ICommandMiddleware>(s => s.GetRequiredService<T>(), lifetime);
            services.Add<IEventMiddleware>(s => s.GetRequiredService<T>(), lifetime);
            services.Add<IQueryMiddleware>(s => s.GetRequiredService<T>(), lifetime);

            return services;
        }

        /// <summary>
        /// Registers the given instance as a <see cref="ICommandMiddleware"/>,
        /// <see cref="IEventMiddleware"/> and <see cref="IQueryMiddleware"/>.
        /// </summary>
        /// <typeparam name="T">The middleware implementation type</typeparam>
        /// <param name="services">The services collection</param>
        /// <param name="instance">The instance to register</param>
        /// <returns>The service collection after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMediatorMiddleware<T>(
            this IServiceCollection services, T instance)
            where T : class, IMiddleware
        {
            services.AddSingleton<ICommandMiddleware>(instance);
            services.AddSingleton<IEventMiddleware>(instance);
            services.AddSingleton<IQueryMiddleware>(instance);

            return services;
        }

        /// <summary>
        /// Registers the given function as a factory for <see cref="ICommandMiddleware"/>,
        /// <see cref="IEventMiddleware"/> and <see cref="IQueryMiddleware"/> instances.
        /// </summary>
        /// <typeparam name="T">The middleware implementation type</typeparam>
        /// <param name="services">The services collection</param>
        /// <param name="factory">The factory method</param>
        /// <param name="lifetime">The middleware lifetime</param>
        /// <returns>The service collection after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMediatorMiddleware<T>(
            this IServiceCollection services, Func<IServiceProvider, T> factory,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where T : class, IMiddleware
        {
            services.Add(factory, lifetime);
            services.Add<ICommandMiddleware>(s => s.GetRequiredService<T>(), lifetime);
            services.Add<IEventMiddleware>(s => s.GetRequiredService<T>(), lifetime);
            services.Add<IQueryMiddleware>(s => s.GetRequiredService<T>(), lifetime);

            return services;
        }

        #endregion
    }
}
