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
using Microsoft.Extensions.DependencyInjection;

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// Mediator options for container registration
    /// </summary>
    public class MediatorOptions
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <exception cref="ArgumentNullException"></exception>
        public MediatorOptions(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        /// <summary>
        /// The service collection
        /// </summary>
        public IServiceCollection Services { get; }

        /// <summary>
        /// The mediator instance lifetime. Defaults to '<see cref="ServiceLifetime.Scoped"/>'.
        /// </summary>
        public ServiceLifetime Lifetime { get; set; } = ServiceLifetime.Scoped;

        /// <summary>
        /// The mediator service provider instance lifetime. Defaults to '<see cref="ServiceLifetime.Scoped"/>'.
        /// </summary>
        public ServiceLifetime ServiceProviderLifetime { get; set; } = ServiceLifetime.Scoped;
    }
}
