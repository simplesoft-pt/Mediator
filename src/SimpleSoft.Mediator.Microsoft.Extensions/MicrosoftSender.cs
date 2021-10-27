using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// Sends commands of a given type into the handler
    /// </summary>
    /// <typeparam name="TCommand">The command type</typeparam>
    public class MicrosoftSender<TCommand> : Sender<TCommand> where TCommand : class, ICommand
    {
        private readonly ILogger<ISender<TCommand>> _logger;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="pipelines"></param>
        /// <param name="logger"></param>
        public MicrosoftSender(
            ICommandHandler<TCommand> handler, 
            IEnumerable<IPipeline> pipelines,
            ILogger<Sender<TCommand>> logger) 
            : base(handler, pipelines)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public override async Task SendAsync(TCommand cmd, CancellationToken ct)
        {
            using (_logger.BeginScope("CommandName:{commandName} CommandId:{commandId}", cmd.GetType().Name, cmd.Id))
            {
                _logger.LogDebug("Sending command");
                await base.SendAsync(cmd, ct).ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    /// Sends commands of a given type into the handler
    /// </summary>
    /// <typeparam name="TCommand">The command type</typeparam>
    /// <typeparam name="TResult">The result type</typeparam>
    public class MicrosoftSender<TCommand, TResult> : Sender<TCommand, TResult> where TCommand : class, ICommand<TResult>
    {
        private readonly ILogger<ISender<TCommand, TResult>> _logger;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="pipelines"></param>
        /// <param name="logger"></param>
        public MicrosoftSender(
            ICommandHandler<TCommand, TResult> handler, 
            IEnumerable<IPipeline> pipelines,
            ILogger<Sender<TCommand, TResult>> logger) 
            : base(handler, pipelines)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public override async Task<TResult> SendAsync(TCommand cmd, CancellationToken ct)
        {
            using (_logger.BeginScope("CommandName:{commandName} CommandId:{commandId}", cmd.GetType().Name, cmd.Id))
            {
                _logger.LogDebug("Sending command");
                return await base.SendAsync(cmd, ct).ConfigureAwait(false);
            }
        }
    }
}
