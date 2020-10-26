using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// Fetches data of a given query type from the handler
    /// </summary>
    /// <typeparam name="TQuery">The query type</typeparam>
    /// <typeparam name="TResult">The result type</typeparam>
    public class MicrosoftFetcher<TQuery, TResult> : Fetcher<TQuery, TResult> where TQuery : class, IQuery<TResult>
    {
        private readonly ILogger<Fetcher<TQuery, TResult>> _logger;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="pipelines"></param>
        /// <param name="logger"></param>
        public MicrosoftFetcher(
            IQueryHandler<TQuery, TResult> handler, 
            IEnumerable<IPipeline> pipelines, 
            ILogger<Fetcher<TQuery, TResult>> logger) 
            : base(handler, pipelines)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public override async Task<TResult> FetchAsync(TQuery query, CancellationToken ct)
        {
            using (_logger.BeginScope("QueryName:{queryName} QueryId:{queryId}", query.GetType().Name, query.Id))
            {
                _logger.LogDebug("Fetching query data");
                return await base.FetchAsync(query, ct);
            }
        }
    }
}
