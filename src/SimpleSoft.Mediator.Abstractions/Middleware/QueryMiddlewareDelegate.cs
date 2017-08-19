using System.Threading;
using System.Threading.Tasks;

namespace SimpleSoft.Mediator.Middleware
{
    /// <summary>
    /// Method invoked when an <see cref="IQuery{TResult}"/> is fetched.
    /// </summary>
    /// <typeparam name="TQuery">The query type</typeparam>
    /// <typeparam name="TResult">The result type</typeparam>
    /// <param name="query">The query to fetch</param>
    /// <param name="ct">The cancellation token</param>
    /// <returns>A task to be awaited for the result</returns>
    public delegate Task<TResult> QueryMiddlewareDelegate<in TQuery, TResult>(TQuery query, CancellationToken ct)
        where TQuery : IQuery<TResult>;
}
