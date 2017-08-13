using System.Threading;
using System.Threading.Tasks;

namespace SimpleSoft.Mediator.Tests.Mocks
{
    public class MockResultCommandHandler : ICommandHandler<MockResultCommand, object>
    {
        public Task<object> HandleAsync(MockResultCommand cmd, CancellationToken ct)
        {
            return Task.FromResult<object>(null);
        }
    }
}