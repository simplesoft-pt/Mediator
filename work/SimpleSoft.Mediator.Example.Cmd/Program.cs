using System;
using Microsoft.Extensions.Logging;

namespace SimpleSoft.Mediator.Example.Cmd
{
    public class Program
    {
        private static readonly ILoggerFactory LoggerFactory;
        private static readonly ILogger<Program> Logger;

        static Program()
        {
            LoggerFactory = new LoggerFactory()
                .AddConsole(LogLevel.Trace, true);
            Logger = LoggerFactory.CreateLogger<Program>();
        }

        public static void Main(string[] args)
        {
            Logger.LogInformation("Application started...");
            try
            {

            }
            catch (Exception e)
            {
                Logger.LogCritical(0, e, "Fatal exception!");
            }
            finally
            {
                Logger.LogInformation("Application terminated. Press <enter> to exit...");
                Console.ReadLine();
            }
        }
    }
}
