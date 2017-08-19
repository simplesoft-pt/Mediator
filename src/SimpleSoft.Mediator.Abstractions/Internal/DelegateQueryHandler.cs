using System;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleSoft.Mediator.Internal
{
    internal sealed class DelegateQueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        private readonly Func<TQuery, CancellationToken, Task<TResult>> _handler;

        public DelegateQueryHandler(Func<TQuery, CancellationToken, Task<TResult>> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            _handler = handler;
        }

        /// <inheritdoc />
        public async Task<TResult> HandleAsync(TQuery query, CancellationToken ct = new CancellationToken())
        {
            return await _handler(query, ct).ConfigureAwait(false);
        }
    }
}
