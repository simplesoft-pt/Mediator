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
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SimpleSoft.Mediator;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configures the mediator into the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="config">An optional configurations action</param>
        /// <returns>The service collection after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMediator(this IServiceCollection services, Action<MediatorOptions> config = null)
        {
            var options = new MediatorOptions(services);
            config?.Invoke(options);

            services.TryAdd(new ServiceDescriptor(
                typeof(IMediatorServiceProvider), typeof(MicrosoftMediatorServiceProvider), options.ServiceProviderLifetime));
            services.TryAdd(new ServiceDescriptor(
                typeof(IMediator), typeof(Mediator), options.Lifetime));

            services.TryAdd(new ServiceDescriptor(
                typeof(IFetcher<,>), typeof(MicrosoftFetcher<,>), ServiceLifetime.Transient));
            services.TryAdd(new ServiceDescriptor(
                typeof(ISender<>), typeof(MicrosoftSender<>), ServiceLifetime.Transient));
            services.TryAdd(new ServiceDescriptor(
                typeof(ISender<,>), typeof(MicrosoftSender<,>), ServiceLifetime.Transient));
            services.TryAdd(new ServiceDescriptor(
                typeof(IBroadcaster<>), typeof(MicrosoftBroadcaster<>), ServiceLifetime.Transient));

            return services;
        }

        /// <summary>
        /// Configures the given type as a mediator <see cref="IPipeline"/>.
        /// Registration order will be kept when executing the pipeline.
        /// </summary>
        /// <typeparam name="T">The pipeline type</typeparam>
        /// <param name="options">The mediator options</param>
        /// <param name="lifetime">The pipeline lifetime</param>
        /// <returns>The options instance after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static MediatorOptions AddPipeline<T>(this MediatorOptions options, ServiceLifetime lifetime = ServiceLifetime.Scoped) 
            where T : class, IPipeline
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            options.Services.Add(new ServiceDescriptor(typeof(IPipeline), typeof(T), lifetime));
            return options;
        }

        /// <summary>
        /// Configures the given factory for mediator <see cref="IPipeline"/> instances.
        /// Registration order will be kept when executing the pipeline.
        /// </summary>
        /// <param name="options">The mediator options</param>
        /// <param name="factory">The pipeline factory</param>
        /// <param name="lifetime">The pipeline lifetime</param>
        /// <returns>The options instance after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static MediatorOptions AddPipeline(
            this MediatorOptions options, Func<IServiceProvider, IPipeline> factory, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            options.Services.Add(new ServiceDescriptor(typeof(IPipeline), factory, lifetime));
            return options;
        }

        /// <summary>
        /// Configures the given instance as a mediator <see cref="IPipeline"/>.
        /// Registration order will be kept when executing the pipeline.
        /// </summary>
        /// <typeparam name="T">The pipeline type</typeparam>
        /// <param name="options">The mediator options</param>
        /// <param name="instance">The singleton instance</param>
        /// <returns>The options instance after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static MediatorOptions AddPipeline<T>(this MediatorOptions options, T instance) 
            where T : class, IPipeline
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            options.Services.Add(new ServiceDescriptor(typeof(IPipeline), instance));
            return options;
        }

        /// <summary>
        /// Registers all the command, query and event handlers found in the given type assembly.
        /// </summary>
        /// <typeparam name="T">The type to scan the assembly</typeparam>
        /// <param name="options">The mediator options</param>
        /// <param name="lifetime">The handlers lifetime</param>
        /// <returns>The options instance after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static MediatorOptions AddHandlersFromAssemblyOf<T>(this MediatorOptions options, 
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
#if NETSTANDARD1_1
            return options.AddHandlersFromAssembly(typeof(T).GetTypeInfo().Assembly);
#else
            return options.AddHandlersFromAssembly(typeof(T).Assembly);
#endif
        }

        /// <summary>
        /// Registers all the command, query and event handlers found in the given type assembly.
        /// </summary>
        /// <typeparam name="T1">The type to scan the assembly</typeparam>
        /// <typeparam name="T2">The type to scan the assembly</typeparam>
        /// <param name="options">The mediator options</param>
        /// <param name="lifetime">The handlers lifetime</param>
        /// <returns>The options instance after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static MediatorOptions AddHandlersFromAssemblyOf<T1, T2>(this MediatorOptions options,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            options.AddHandlersFromAssemblyOf<T1>(lifetime);
            options.AddHandlersFromAssemblyOf<T2>(lifetime);

            return options;
        }

        /// <summary>
        /// Registers all the command, query and event handlers found in the given type assembly.
        /// </summary>
        /// <typeparam name="T1">The type to scan the assembly</typeparam>
        /// <typeparam name="T2">The type to scan the assembly</typeparam>
        /// <typeparam name="T3">The type to scan the assembly</typeparam>
        /// <param name="options">The mediator options</param>
        /// <param name="lifetime">The handlers lifetime</param>
        /// <returns>The options instance after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static MediatorOptions AddHandlersFromAssemblyOf<T1, T2, T3>(this MediatorOptions options,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            options.AddHandlersFromAssemblyOf<T1>(lifetime);
            options.AddHandlersFromAssemblyOf<T2>(lifetime);
            options.AddHandlersFromAssemblyOf<T3>(lifetime);

            return options;
        }

        /// <summary>
        /// Registers all the command, query and event handlers found in the given assembly.
        /// </summary>
        /// <param name="options">The mediator options</param>
        /// <param name="assembly">The assembly to scan</param>
        /// <param name="lifetime">The handlers lifetime</param>
        /// <returns>The options instance after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static MediatorOptions AddHandlersFromAssembly(this MediatorOptions options, 
            Assembly assembly, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));

#if NETSTANDARD1_1
            var exportedTypes = assembly.ExportedTypes.Select(t => t.GetTypeInfo());
#else
            var exportedTypes = assembly.ExportedTypes;
#endif
            foreach (var t in exportedTypes.Where(t => t.IsClass && !t.IsAbstract))
            {
#if NETSTANDARD1_1
                var implementedInterfaces = t.ImplementedInterfaces.Select(i => i.GetTypeInfo());
#else
                var implementedInterfaces = t.GetInterfaces();
#endif
                foreach (var i in implementedInterfaces.Where(e => e.IsGenericType))
                {
                    var iGenericType = i.GetGenericTypeDefinition();
                    if (iGenericType == typeof(ICommandHandler<>) ||
                        iGenericType == typeof(ICommandHandler<,>) ||
                        iGenericType == typeof(IEventHandler<>) ||
                        iGenericType == typeof(IQueryHandler<,>))
                    {
#if NETSTANDARD1_1
                        var serviceType = i.AsType();
                        var implementationType = t.AsType();
#else
                        var serviceType = i;
                        var implementationType = t;
#endif
                        options.Services.Add(new ServiceDescriptor(serviceType, implementationType, lifetime));
                    }
                }
            }

            return options;
        }

        /// <summary>
        /// Registers all the command, query and event handlers found in the given assemblies
        /// as a <see cref="ServiceLifetime.Scoped"/>.
        /// </summary>
        /// <param name="options">The mediator options</param>
        /// <param name="assembly1">The assembly to scan</param>
        /// <param name="assembly2">The assembly to scan</param>
        /// <param name="assemblies">The assemblies to scan</param>
        /// <returns>The options instance after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static MediatorOptions AddHandlersFromAssembly(this MediatorOptions options,
            Assembly assembly1, Assembly assembly2, params Assembly[] assemblies)
        {
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));

            options.AddHandlersFromAssembly(assembly1);
            options.AddHandlersFromAssembly(assembly2);

            foreach (var a in assemblies)
                options.AddHandlersFromAssembly(a);

            return options;
        }
    }
}
