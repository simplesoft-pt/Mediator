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
        private static readonly Type MediatorType = typeof(IMediator);

        /// <summary>
        /// The default builder delegate for <see cref="IMediator"/> instances. It
        /// builds instances of <see cref="Mediator"/>.
        /// </summary>
        public static readonly Func<IServiceProvider, IMediator> DefaultMediatorBuilder =
            s => new Mediator(s.GetRequiredService<IMediatorFactory>());

        /// <summary>
        /// The service descriptor for the <see cref="IMediator"/> instance.
        /// By default it uses the <see cref="DefaultMediatorBuilder"/> delegate.
        /// </summary>
        public ServiceDescriptor MediatorDescriptor { get; private set; }
            = new ServiceDescriptor(MediatorType, DefaultMediatorBuilder, ServiceLifetime.Singleton);

        /// <summary>
        /// Uses the given <see cref="IMediator"/> instance.
        /// </summary>
        /// <param name="instance">The instance to use</param>
        /// <returns>The mediator options after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public MediatorOptions UseMediator(IMediator instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            MediatorDescriptor = new ServiceDescriptor(MediatorType, instance);
            return this;
        }

        /// <summary>
        /// Uses the given type as the <see cref="IMediator"/> implementation.
        /// </summary>
        /// <typeparam name="TMediator">The mediator type</typeparam>
        /// <param name="lifetime">The instance lifetime</param>
        /// <returns>The mediator options after changes</returns>
        public MediatorOptions UseMediator<TMediator>(ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where TMediator : IMediator
        {
            MediatorDescriptor = new ServiceDescriptor(MediatorType, typeof(TMediator), lifetime);
            return this;
        }

        /// <summary>
        /// Uses the given factory to build <see cref="IMediator"/> instances.
        /// </summary>
        /// <param name="factory">The factory to use</param>
        /// <param name="lifetime">The instance lifetime</param>
        /// <returns>The mediator options after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public MediatorOptions UseMediator(Func<IServiceProvider, IMediator> factory, ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            MediatorDescriptor = new ServiceDescriptor(MediatorType, factory, lifetime);
            return this;
        }
    }
}