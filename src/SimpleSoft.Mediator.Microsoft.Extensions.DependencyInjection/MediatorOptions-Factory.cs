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
    /// <summary>
    /// The mediator registration options
    /// </summary>
    public partial class MediatorOptions
    {
        private static readonly Type FactoryType = typeof(IMediatorFactory);

        /// <summary>
        /// The service descriptor for the <see cref="IMediatorFactory"/> instance.
        /// By default it will build a singleton <see cref="MicrosoftMediatorFactory"/> instance.
        /// </summary>
        public ServiceDescriptor FactoryDescriptor { get; private set; }
            = new ServiceDescriptor(FactoryType, typeof(MicrosoftMediatorFactory), ServiceLifetime.Singleton);

        /// <summary>
        /// Uses the given <see cref="IMediatorFactory"/> instance.
        /// </summary>
        /// <param name="instance">The instance to use</param>
        /// <returns>The mediator options after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public MediatorOptions UseFactory(IMediatorFactory instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            FactoryDescriptor = new ServiceDescriptor(FactoryType, instance);
            return this;
        }

        /// <summary>
        /// Uses the given type as the <see cref="IMediatorFactory"/> implementation.
        /// </summary>
        /// <typeparam name="TFactory">The factory type</typeparam>
        /// <param name="lifetime">The instance lifetime</param>
        /// <returns>The mediator options after changes</returns>
        public MediatorOptions UseFactory<TFactory>(ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where TFactory : IMediatorFactory
        {
            FactoryDescriptor = new ServiceDescriptor(FactoryType, typeof(TFactory), lifetime);
            return this;
        }

        /// <summary>
        /// Uses the given factory to build <see cref="IMediatorFactory"/> instances.
        /// </summary>
        /// <param name="factory">The factory to use</param>
        /// <param name="lifetime">The instance lifetime</param>
        /// <returns>The mediator options after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public MediatorOptions UseFactory(Func<IServiceProvider, IMediatorFactory> factory, ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            FactoryDescriptor = new ServiceDescriptor(FactoryType, factory, lifetime);
            return this;
        }
    }
}