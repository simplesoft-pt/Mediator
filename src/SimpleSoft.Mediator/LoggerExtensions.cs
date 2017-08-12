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

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Logging
{
    internal static class LoggerExtensions
    {
        /// <summary>
        /// Used to reduce args allocation when using logger methods
        /// </summary>
        public static readonly object[] EmptyObjectArray = new object[0];

        public static void LogTrace(this ILogger logger, string message)
        {
            logger.LogTrace(message, EmptyObjectArray);
        }

        public static void LogDebug(this ILogger logger, string message)
        {
            logger.LogDebug(message, EmptyObjectArray);
        }

        public static void LogInformation(this ILogger logger, string message)
        {
            logger.LogInformation(message, EmptyObjectArray);
        }

        public static void LogWarning(this ILogger logger, string message)
        {
            logger.LogWarning(message, EmptyObjectArray);
        }

        public static void LogError(this ILogger logger, string message)
        {
            logger.LogError(message, EmptyObjectArray);
        }

        public static void LogCritical(this ILogger logger, string message)
        {
            logger.LogCritical(message, EmptyObjectArray);
        }
    }
}