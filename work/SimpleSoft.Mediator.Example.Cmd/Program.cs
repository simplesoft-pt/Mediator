using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleSoft.Hosting;
using SimpleSoft.Hosting.Params;

namespace SimpleSoft.Mediator.Example.Cmd
{
    public class Program
    {
        private static readonly CancellationTokenSource TokenSource;

        static Program()
        {
            TokenSource = new CancellationTokenSource();
            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                TokenSource.Cancel();
                eventArgs.Cancel = true;
            };
        }

        public static void Main(string[] args)
        {
            var loggerFactory = new LoggerFactory()
                .AddConsole(LogLevel.Trace, true);

            var logger = loggerFactory.CreateLogger<Program>();
            logger.LogInformation("Application started...");
            try
            {
                using (var hostBuilder = new HostBuilder()
                    .UseLoggerFactory(loggerFactory)
                    .UseStartup<ExampleStartup>())
                {
                    hostBuilder.RunHostAsync<ExampleHost>(TokenSource.Token)
                        .ConfigureAwait(false).GetAwaiter().GetResult();
                }
            }
            catch (TaskCanceledException)
            {
                logger.LogWarning("The code execution has been canceled.");
            }
            catch (Exception e)
            {
                logger.LogCritical(0, e, "Fatal exception!");
            }
            finally
            {
                logger.LogInformation("Application terminated. Press <enter> to exit...");
                Console.ReadLine();
            }
        }

        private class ExampleStartup : HostStartup
        {
            public override void ConfigureServiceCollection(IServiceCollectionHandlerParam param) =>
                param.ServiceCollection
                    .AddSingleton<IDictionary<Guid, User>>(s => new Dictionary<Guid, User>())
                    .AddMediator(options =>
                    {
                        options
                            .UseFactory(
                                s => MediatorOptions.DefaultFactoryBuilder(s)
                                    .UsingLogger(s.GetRequiredService<ILogger<IMediatorFactory>>()))
                            .UseMediator<LoggingMediator.Default>();

                        options
                            .AddMiddleware<LoggingMiddleware>(ServiceLifetime.Singleton)
                            .AddMiddleware<IgnoreHandlerNotFoundExceptionMiddleware>(ServiceLifetime.Singleton);
                    })
                    .AddMediatorHandlerForCommand<RegisterUserCommand, Guid, RegisterUserCommandHandler>(ServiceLifetime.Transient)
                    .AddMediatorHandlerForCommand<ChangeUserPasswordCommand, ChangeUserPasswordCommandHandler>(ServiceLifetime.Transient)
                    .AddMediatorHandlerForQuery<UserByIdQuery, User, UserByIdQueryHandler>(ServiceLifetime.Transient);

            public override IServiceProvider BuildServiceProvider(IServiceProviderBuilderParam param) =>
                param.ServiceCollection.BuildServiceProvider(true);
        }

        private class ExampleHost : IHost
        {
            private readonly ILogger<ExampleHost> _logger;
            private readonly IMediator _mediator;

            public ExampleHost(ILogger<ExampleHost> logger, IMediator mediator)
            {
                _logger = logger;
                _mediator = mediator;
            }

            public async Task RunAsync(CancellationToken ct)
            {
                for (var i = 0; i < 20; i++)
                {
                    _logger.LogDebug("Creating new user");
                    var userId = await _mediator.SendAsync<RegisterUserCommand, Guid>(
                        new RegisterUserCommand(Guid.NewGuid(), $"someuser{i:D2}@domain.com", "123456"), ct);

                    _logger.LogDebug("Getting user '{userId}'", userId);
                    var user = await _mediator.FetchAsync<UserByIdQuery, User>(
                        new UserByIdQuery(Guid.NewGuid(), userId), ct);

                    if (user == null)
                    {
                        _logger.LogDebug("User '{userId}' could not be found");
                    }
                    else
                    {
                        _logger.LogDebug("Changing password for user '{userId}'", userId);
                        await _mediator.SendAsync(new ChangeUserPasswordCommand(
                            Guid.NewGuid(), userId, "123456", "654321"), ct);
                    }
                }
            }
        }
    }
}
