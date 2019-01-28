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

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Logging
{
    internal static class LoggerExtensions
    {
        private static readonly object[] EmptyArgs = new object[0];

        internal static IDisposable BeginScope(this ILogger logger, string message) =>
            logger.BeginScope(message, EmptyArgs);

        internal static IDisposable BeginScope<T1>(this ILogger logger, string message, T1 arg1) =>
            logger.BeginScope(message, new object[]
            {
                arg1?.ToString()
            });

        internal static IDisposable BeginScope<T1, T2>(this ILogger logger, string message, T1 arg1, T2 arg2) =>
            logger.BeginScope(message, new object[]
            {
                arg1?.ToString(),
                arg2?.ToString()
            });

        internal static IDisposable BeginScope<T1, T2, T3>(this ILogger logger, string message, T1 arg1, T2 arg2, T3 arg3) =>
            logger.BeginScope(message, new object[]
            {
                arg1?.ToString(),
                arg2?.ToString(),
                arg3?.ToString()
            });

        internal static void LogDebug(this ILogger logger, string message)
        {
            if (logger.IsEnabled(LogLevel.Debug))
                logger.LogDebug(message, EmptyArgs);
        }

        internal static void LogDebug<T1>(this ILogger logger, string message, T1 arg1)
        {
            if (logger.IsEnabled(LogLevel.Debug))
                logger.LogDebug(message, new object[]
                {
                    arg1?.ToString()
                });
        }

        internal static void LogDebug<T1, T2>(this ILogger logger, string message, T1 arg1, T2 arg2)
        {
            if (logger.IsEnabled(LogLevel.Debug))
                logger.LogDebug(message, new object[]
                {
                    arg1?.ToString(),
                    arg2?.ToString()
                });
        }

        internal static void LogDebug<T1, T2, T3>(this ILogger logger, string message, T1 arg1, T2 arg2, T3 arg3)
        {
            if (logger.IsEnabled(LogLevel.Debug))
                logger.LogDebug(message, new object[]
                {
                    arg1?.ToString(),
                    arg2?.ToString(),
                    arg3?.ToString()
                });
        }
    }
}
