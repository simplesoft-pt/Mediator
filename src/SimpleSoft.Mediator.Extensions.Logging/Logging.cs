using Microsoft.Extensions.Logging;

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// Aggregates mediator wrappers that support logging
    /// </summary>
    public static partial class Logging
    {
        internal static readonly object[] EmptyObjectArray = new object[0];

        internal static void LogDebug(this ILogger logger, string message)
        {
            logger.LogDebug(message, EmptyObjectArray);
        }
    }
}