using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleSoft.Mediator.Example.Cmd.Queries
{
    public class UserByIdQueryHandler : IQueryHandler<UserByIdQuery, User>
    {
        private readonly IDictionary<Guid, User> _store;

        public UserByIdQueryHandler(IDictionary<Guid, User> store)
        {
            _store = store;
        }

        public async Task<User> HandleAsync(UserByIdQuery query, CancellationToken ct)
        {
            await Task.Delay(1000, ct);

            User user;
            return _store.TryGetValue(query.UserId, out user) ? user : null;
        }
    }
}