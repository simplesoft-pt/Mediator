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
                typeof(IMediator), typeof(MicrosoftMediator), options.Lifetime));

            return services;
        }

        /// <summary>
        /// Configures the given type as a mediator <see cref="IPipeline"/>.
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
    }
}
