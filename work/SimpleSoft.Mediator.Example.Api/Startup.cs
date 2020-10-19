using System.Linq;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleSoft.Mediator.Example.Api.Database;

namespace SimpleSoft.Mediator.Example.Api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMediator(o =>
            {
                // order of pipeline execution is the same as the order of registration
                // in this case
                // logging -> validation -> transaction -> handlers

                o.AddPipelineForLogging(cfg =>
                {
                    cfg.Level = LogLevel.Debug;

                    // will also serialize command results as JSON into the log
                    cfg.LogCommandResult = true;
                });
                o.AddPipelineForValidation(cfg =>
                {
                    // will search for IValidator<TCommand> from the container
                    // and will fail if something is invalid
                    cfg.ValidateCommand = true;
                });
                o.AddPipelineForEFCoreTransaction<ExampleApiContext>(cfg =>
                {
                    // enforcing that a database transaction wraps all command executions
                    //
                    // await using var tx = await _context.Database.BeginTransactionAsync(ct);
                    // ... next pipelines or you command
                    // await _context.SaveChangesAsync(ct);
                    // await tx.CommitAsync(ct);
                    //
                    // remarks: remember that updates and removes only happen on save changes
                    //  if your command needs to return something, like an updated RowVersion, you still need to
                    //  call SaveChanges before exiting the command
                    cfg.BeginTransactionOnCommand = true;
                });

                // searches for all ICommandHandler, IQueryHandler and IEventHandler
                // and adds them into the IServiceCollection
                o.AddHandlersFromAssemblyOf<Startup>();
            });

            // scanning for validators into the container
            foreach (var implementationType in typeof(Startup).Assembly.ExportedTypes.Where(t => t.IsClass && !t.IsAbstract))
            foreach (var serviceType in implementationType.GetInterfaces()
                .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IValidator<>)))
                services.AddSingleton(serviceType, implementationType);

            services.AddDbContext<ExampleApiContext>(o =>
            {
                o.UseInMemoryDatabase("ExampleApi")
                    .ConfigureWarnings(warn =>
                    {
                        // since InMemoryDatabase does not support transactions
                        // for test purposes we are going to ignore
                        warn.Ignore(InMemoryEventId.TransactionIgnoredWarning);
                    });
            });

            services
                .AddSwaggerGen()
                .AddMvcCore()
                .AddApiExplorer();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mediator ExampleApi V1");
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
