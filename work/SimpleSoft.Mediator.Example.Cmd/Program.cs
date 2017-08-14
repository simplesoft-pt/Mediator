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
                mediator.PublishAsync<RegisterUserCommand, Guid>(new RegisterUserCommand(
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
                .AddSingleton<IHandlerFactory>(s => new DelegateHandlerFactory(s.GetService, s.GetServices))
                .AddSingleton<IMediator, Mediator>();

                // this should be a class implementing ICommandHandler<RegisterUserCommand>
            serviceCollection    
                .AddTransient(s => DelegateHandler.Command<RegisterUserCommand, Guid>((cmd, ct) =>
                {
                    var store = s.GetRequiredService<IDictionary<Guid, User>>();
                    if (store.Values.Any(e => e.Email.Equals(cmd.Email, StringComparison.OrdinalIgnoreCase)))
                    {
                        // this could be a fail event instead
                        //  eg. await _mediator.BroadcastAsync(new CommandFailedEvent(cmd.Id), ct);
                        throw new InvalidOperationException("Duplicated user email");
                    }

                    var userId = Guid.NewGuid();
                    store.Add(userId, new User
                    {
                        Email = cmd.Email,
                        Password = cmd.Password
                    });

                    //  this could be a UserCreatedEvent instead
                    //  eg. await _mediator.BroadcastAsync(new UserCreatedEvent(userId), ct);
                    return Task.FromResult(userId);
                }));

            // this should be a class implementing ICommandHandler<ChangeUserPasswordCommand>
            serviceCollection
                .AddTransient(s => DelegateHandler.Command<ChangeUserPasswordCommand>((cmd, ct) =>
                {
                    var store = s.GetRequiredService<IDictionary<Guid, User>>();
                    User user;
                    if (store.TryGetValue(cmd.UserId, out user))
                    {
                        if (user.Password.Equals(cmd.OldPassword))
                        {
                            user.Password = cmd.NewPassword;
                            return Task.CompletedTask;
                        }

                        throw new InvalidOperationException($"Invalid password for user id '{cmd.UserId}'");
                    }

                    throw new InvalidOperationException($"User id '{cmd.UserId}' could not be found");
                }));

            return serviceCollection.BuildServiceProvider(true);
        }
    }
}
