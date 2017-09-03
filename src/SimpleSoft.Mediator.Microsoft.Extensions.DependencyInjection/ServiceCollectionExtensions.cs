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

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configures the mediator into the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="config">An optional configuration delegate</param>
        /// <returns>The service collection after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMediator(
            this IServiceCollection services, Action<MediatorOptions> config = null)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var options = new MediatorOptions();
            config?.Invoke(options);

            services.TryAdd(options.MediatorDescriptor);
            services.TryAdd(options.FactoryDescriptor);

            services.Add(options.CommandMiddlewareDescriptors);
            services.Add(options.EventMiddlewareDescriptors);
            services.Add(options.QueryMiddlewareDescriptors);

            return services;
        }

        internal static IServiceCollection Add<TService, TImpl>(this IServiceCollection services, ServiceLifetime lifetime)
            where TImpl : TService
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.Add(new ServiceDescriptor(typeof(TService), typeof(TImpl), lifetime));

            return services;
        }

        internal static IServiceCollection Add<TService>(this IServiceCollection services,
            Func<IServiceProvider, TService> factory, ServiceLifetime lifetime)
            where TService : class
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            services.Add(new ServiceDescriptor(typeof(TService), factory, lifetime));

            return services;
        }
    }
}
