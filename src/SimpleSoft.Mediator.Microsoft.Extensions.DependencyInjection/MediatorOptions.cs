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
    public class MediatorOptions
    {
        private Func<IServiceProvider, IMediator> _mediatorBuilder =
            provider => new Mediator(provider.GetRequiredService<IMediatorFactory>());

        private Func<IServiceProvider, IMediatorFactory> _factoryBuilder =
            provider => new MicrosoftMediatorFactory(provider);

        /// <summary>
        /// The function that builds the <see cref="IMediatorFactory"/> to be used. By default,
        /// it will build a <see cref="MicrosoftMediatorFactory"/> instance.
        /// </summary>
        public Func<IServiceProvider, IMediatorFactory> FactoryBuilder
        {
            get { return _factoryBuilder; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                _factoryBuilder = value;
            }
        }

        /// <summary>
        /// The lifetime of the <see cref="IMediatorFactory"/> instance. By default,
        /// the value is <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public ServiceLifetime FactoryLifetime { get; set; } = ServiceLifetime.Singleton;

        /// <summary>
        /// The function that builds the <see cref="IMediator"/> to be used. By default,
        /// it will build a <see cref="Mediator"/> instance.
        /// </summary>
        public Func<IServiceProvider, IMediator> MediatorBuilder
        {
            get { return _mediatorBuilder; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                _mediatorBuilder = value;
            }
        }

        /// <summary>
        /// The lifetime of the <see cref="IMediator"/> instance. By default,
        /// the value is <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public ServiceLifetime MediatorLifetime { get; set; } = ServiceLifetime.Singleton;
    }
}