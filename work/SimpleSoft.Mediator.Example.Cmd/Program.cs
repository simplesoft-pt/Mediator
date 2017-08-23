using System;
using System.Collections.Generic;
using System.Linq;
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
                .AddMediator(options =>
                {
                    var factoryBuilder = options.FactoryBuilder;
                    options.FactoryBuilder = s => new LoggingMediatorFactory(
                        factoryBuilder(s), s.GetRequiredService<ILogger<LoggingMediatorFactory>>());

                    var mediatorBuilder = options.MediatorBuilder;
                    options.MediatorBuilder = s => new LoggingMediator(
                        mediatorBuilder(s), s.GetRequiredService<ILogger<LoggingMediator.Default>>());
                })
                .AddSingleton<Application>();

            services
                .AddMediatorMiddleware<LoggingMiddleware>(ServiceLifetime.Singleton)
                .AddMediatorMiddleware<IgnoreHandlerNotFoundExceptionMiddleware>(ServiceLifetime.Singleton);

            // this should be a class implementing ICommandHandler<RegisterUserCommand>
            services.AddMediatorHandlerForCommand(
                s => DelegateHandler.Command<RegisterUserCommand, Guid>(async (cmd, ct) =>
                {
                    var store = s.GetRequiredService<IDictionary<Guid, User>>();
                    if (store.Values.Any(e => e.Email.Equals(cmd.Email, StringComparison.OrdinalIgnoreCase)))
                    {
                        // this could be a fail event instead
                        //  eg. await _mediator.BroadcastAsync(new CommandFailedEvent(cmd.Id), ct);
                        throw new InvalidOperationException("Duplicated user email");
                    }

                    await Task.Delay(1000, ct);

                    var userId = Guid.NewGuid();
                    store.Add(userId, new User
                    {
                        Email = cmd.Email,
                        Password = cmd.Password
                    });

                    //  this could be a UserCreatedEvent instead of a command returning a result
                    //  eg. await _mediator.BroadcastAsync(new UserCreatedEvent(userId), ct);
                    return userId;
                }), ServiceLifetime.Transient);

            // this should be a class implementing ICommandHandler<ChangeUserPasswordCommand>
            services.AddMediatorHandlerForCommand(
                s => DelegateHandler.Command<ChangeUserPasswordCommand>(async (cmd, ct) =>
                {
                    var store = s.GetRequiredService<IDictionary<Guid, User>>();
                    User user;
                    if (store.TryGetValue(cmd.UserId, out user))
                    {
                        if (user.Password.Equals(cmd.OldPassword))
                        {
                            await Task.Delay(1000, ct);

                            user.Password = cmd.NewPassword;
                            return;
                        }

                        throw new InvalidOperationException($"Invalid password for user id '{cmd.UserId}'");
                    }

                    throw new InvalidOperationException($"User id '{cmd.UserId}' could not be found");
                }), ServiceLifetime.Transient);

            // this should be a class implementing IQueryHandler<UserByIdQuery,User>
            services.AddMediatorHandlerForQuery(
                s => DelegateHandler.Query<UserByIdQuery, User>(async (query, ct) =>
                {
                    var store = s.GetRequiredService<IDictionary<Guid, User>>();

                    await Task.Delay(1000, ct);

                    User user;
                    return store.TryGetValue(query.UserId, out user) ? user : null;
                }), ServiceLifetime.Transient);

            return services.BuildServiceProvider(true);
        }
    }
}
