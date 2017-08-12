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