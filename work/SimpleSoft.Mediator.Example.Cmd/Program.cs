using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SimpleSoft.Mediator.Example.Cmd
{
    public class Program
    {
        private static readonly CancellationTokenSource TokenSource = new CancellationTokenSource();
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
                Console.CancelKeyPress += (sender, eventArgs) =>
                {
                    TokenSource.Cancel();
                    eventArgs.Cancel = true;
                };

                BuildServiceProvider().GetRequiredService<Application>()
                    .RunAsync(TokenSource.Token).ConfigureAwait(false)
                    .GetAwaiter().GetResult();
            }
            catch (TaskCanceledException)
            {
                Logger.LogWarning("The code execution has been canceled.");
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
            var services = new ServiceCollection()
                .AddSingleton(LoggerFactory)
                .AddLogging()
                .AddSingleton<IDictionary<Guid, User>>(s => new Dictionary<Guid, User>())
                .AddSingleton<Application>();

            services = ConfigureMediator(services);
            
            return services.BuildServiceProvider(true);
        }

        private static IServiceCollection ConfigureMediator(IServiceCollection services)
        {
            services.AddMediator(options =>
            {
                options
                    .UseFactory(
                        s => MediatorOptions.DefaultFactoryBuilder(s)
                            .UsingLogger(s.GetRequiredService<ILogger<IMediatorFactory>>()))
                    .UseMediator<LoggingMediator.Default>();

                options
                    .AddMiddleware<LoggingMiddleware>(ServiceLifetime.Singleton)
                    .AddMiddleware<IgnoreHandlerNotFoundExceptionMiddleware>(ServiceLifetime.Singleton);
            });

            services
                .AddMediatorHandlerForCommand<RegisterUserCommand, Guid, RegisterUserCommandHandler>(ServiceLifetime.Transient)
                .AddMediatorHandlerForCommand<ChangeUserPasswordCommand, ChangeUserPasswordCommandHandler>(ServiceLifetime.Transient)
                .AddMediatorHandlerForQuery<UserByIdQuery, User, UserByIdQueryHandler>(ServiceLifetime.Transient);

            return services;
        }
    }
}
