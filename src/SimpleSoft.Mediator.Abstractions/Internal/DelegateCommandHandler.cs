using System;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleSoft.Mediator.Internal
{
    internal class DelegateCommandHandler<TCommand> : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        private readonly Func<TCommand, CancellationToken, Task> _handler;

        public DelegateCommandHandler(Func<TCommand, CancellationToken, Task> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            _handler = handler;
        }

        public async Task HandleAsync(TCommand cmd, CancellationToken ct = default(CancellationToken))
        {
            await _handler(cmd, ct).ConfigureAwait(false);
        }
    }

    internal class DelegateCommandHandler<TCommand, TResult> : ICommandHandler<TCommand, TResult>
        where TCommand : ICommand<TResult>
    {
        private readonly Func<TCommand, CancellationToken, Task<TResult>> _handler;

        public DelegateCommandHandler(Func<TCommand, CancellationToken, Task<TResult>> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            _handler = handler;
        }

        public async Task<TResult> HandleAsync(TCommand cmd, CancellationToken ct = default(CancellationToken))
        {
            return await _handler(cmd, ct).ConfigureAwait(false);
        }
    }
}