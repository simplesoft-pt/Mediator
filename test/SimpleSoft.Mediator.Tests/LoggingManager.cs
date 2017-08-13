using Microsoft.Extensions.Logging;

namespace SimpleSoft.Mediator.Tests
{
    public static class LoggingManager
    {
        private static readonly ILoggerFactory LoggerFactory;

        static LoggingManager()
        {
            LoggerFactory = new LoggerFactory()
                .AddConsole(LogLevel.Trace, true)
                .AddDebug(LogLevel.Trace);
        }

        public static ILogger<T> CreateTestLogger<T>()
        {
            return LoggerFactory.CreateLogger<T>();
        }

        public static ILogger CreateTestLogger(string categoryName)
        {
            return LoggerFactory.CreateLogger(categoryName);
        }
    }
}
