using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SimpleSoft.Mediator.Example.Cmd.Pipelines;

namespace SimpleSoft.Mediator.Example.Cmd
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await new HostBuilder()
                .ConfigureLogging(b => b
                    .AddConsole(o => o.IncludeScopes = true)
                    .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((ctx, services) =>
                {
                    services.AddMediator(o =>
                    {
                        // pipeline order:
                        //  mediator -> logging -> validation -> transaction -> handler
                        o.AddPipeline<LoggingPipeline>(ServiceLifetime.Singleton);
                        o.AddPipeline<ValidationPipeline>();
                        o.AddPipeline<TransactionPipeline>();
                    });
                })
                .RunConsoleAsync();
        }
    }
}
