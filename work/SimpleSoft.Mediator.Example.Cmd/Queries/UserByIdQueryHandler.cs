using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SimpleSoft.Mediator.Example.Cmd.Queries
{
    public class UserByIdQueryHandler : IQueryHandler<UserByIdQuery, User>
    {
        private readonly ConcurrentDictionary<string, User> _store;
        private readonly ILogger<UserByIdQueryHandler> _logger;

        public UserByIdQueryHandler(ConcurrentDictionary<string, User> store,
            ILogger<UserByIdQueryHandler> logger)
        {
            _store = store;
            _logger = logger;
        }

        public Task<User> HandleAsync(UserByIdQuery query, CancellationToken ct)
        {
            _logger.LogDebug("Searching for user '{userId}'", query.UserId);

            var user = _store.Values.SingleOrDefault(e => e.Id == query.UserId);
            return Task.FromResult(user);
        }
    }
}