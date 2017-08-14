using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleSoft.Mediator.Example.Cmd.Middleware;
using SimpleSoft.Mediator.Example.Cmd.Middleware.Handlers;

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
                var serviceProvider = BuildServiceProvider();

                var mediator = serviceProvider.GetRequiredService<IMediator>();
                mediator.PublishAsync(new RegisterUserCommand(
                        Guid.NewGuid(), "someone@domain.com", "123456"), CancellationToken.None)
                    .ConfigureAwait(false).GetAwaiter().GetResult();
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

        private static IServiceProvider BuildServiceProvider()
        {
            var serviceCollection = new ServiceCollection()
                .AddSingleton(LoggerFactory)
                .AddLogging()
                .AddSingleton<IDictionary<Guid, User>>(s => new Dictionary<Guid, User>())
                .AddTransient<ICommandHandler<RegisterUserCommand>, UserCommandHandler>()
                .AddTransient<ICommandHandler<ChangeUserPasswordCommand>, UserCommandHandler>()
                .AddSingleton<IHandlerFactory>(s => new DelegateHandlerFactory(s.GetService, s.GetServices))
                .AddSingleton<IMediator, Mediator>();

            return serviceCollection.BuildServiceProvider(true);
        }
    }
}
