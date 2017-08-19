using System.Threading;
using System.Threading.Tasks;

namespace SimpleSoft.Mediator.Middleware
{
    /// <summary>
    /// Handling middleware that can be used to intercept queries
    /// </summary>
    public interface IQueryMiddleware
    {
        /// <summary>
        /// Method invoked when an <see cref="IQuery{TResult}"/> is fetched.
        /// </summary>
        /// <typeparam name="TQuery">The query type</typeparam>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="next">The next middleware into the chain</param>
        /// <param name="query">The query to fetch</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited for the result</returns>
        Task<TResult> OnQueryAsync<TQuery, TResult>(QueryMiddlewareDelegate<TQuery, TResult> next, TQuery query, CancellationToken ct)
            where TQuery : IQuery<TResult>;
    }
}