using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SimpleSoft.Mediator.Example.Cmd.Commands;
using SimpleSoft.Mediator.Example.Cmd.Pipelines;
using SimpleSoft.Mediator.Example.Cmd.Queries;

namespace SimpleSoft.Mediator.Example.Cmd
{
    public class Program : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public static async Task Main(string[] args)
        {
            await new HostBuilder()
                .ConfigureLogging(b => b
                    .AddConsole(o => o.IncludeScopes = true)
                    .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((ctx, services) =>
                {
                    //  simulating some database store
                    services.AddSingleton<ConcurrentDictionary<string, User>>();
                    services.AddScoped<UsersTest>();

                    services.AddMediator(o =>
                    {
                        // pipeline order:
                        //  mediator -> logging -> validation -> transaction -> handler
                        o.AddPipeline<LoggingPipeline>(ServiceLifetime.Singleton);
                        o.AddPipeline<ValidationPipeline>();
                        o.AddPipeline<TransactionPipeline>();

                        //  will register all handlers from typeof(Program).Assembly
                        o.AddHandlersFromAssemblyOf<Program>();
                    });
                    services.Scan(s => s.FromAssemblyOf<Program>()
                        .AddClasses(classes => classes.AssignableTo(typeof(IValidator<>)))
                        .AsImplementedInterfaces()
                        .WithScopedLifetime());

                    services.AddHostedService<Program>();
                })
                .RunConsoleAsync();
        }

        public Program(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            //  required for test purposes only because are using Scoped lifetime
            //  ex: in ASP.NET Core all controllers are scoped, this wouldn't be needed
            using (var scope = _scopeFactory.CreateScope())
            {
                var test = scope.ServiceProvider.GetRequiredService<UsersTest>();
                await test.RunAsync(cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private class UsersTest
        {
            private readonly IMediator _mediator;

            public UsersTest(IMediator mediator)
            {
                _mediator = mediator;
            }

            public async Task RunAsync(CancellationToken ct)
            {
                var userId = await _mediator.SendAsync<CreateUserCommand, Guid>(
                    new CreateUserCommand("john.doe@email.com", "John Doe"), ct);

                var user = await _mediator.FetchAsync<UserByIdQuery, User>(
                    new UserByIdQuery(userId), ct);

                await _mediator.SendAsync(
                    new DeleteUserCommand(user.Email), ct);
            }
        }
    }
}
