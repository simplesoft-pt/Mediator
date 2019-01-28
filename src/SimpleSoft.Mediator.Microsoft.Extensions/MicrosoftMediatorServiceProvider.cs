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
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// Factory for mediator dependencies that build services directly from
    /// the <see cref="IServiceProvider"/> instance.
    /// </summary>
    public class MicrosoftMediatorServiceProvider : IMediatorServiceProvider
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger<MicrosoftMediatorServiceProvider> _logger;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="provider">The service provider</param>
        /// <param name="logger">The factory logger</param>
        /// <exception cref="ArgumentNullException"></exception>
        public MicrosoftMediatorServiceProvider(IServiceProvider provider, ILogger<MicrosoftMediatorServiceProvider> logger)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public T BuildService<T>() where T : class
        {
            if (_logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("Building service for type '{type}'", typeof(T));

            return _provider.GetService<T>();
        }

        /// <inheritdoc />
        public IEnumerable<T> BuildServices<T>() where T : class
        {
            if (_logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("Building services for type '{type}'", typeof(T));

            return _provider.GetServices<T>();
        }
    }
}
