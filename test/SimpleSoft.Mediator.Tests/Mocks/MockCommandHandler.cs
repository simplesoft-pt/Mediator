using System.Threading;
using System.Threading.Tasks;

namespace SimpleSoft.Mediator.Tests.Mocks
{
    public class MockCommandHandler : ICommandHandler<MockCommand>
    {
        public Task HandleAsync(MockCommand cmd, CancellationToken ct)
        {
            return Task.CompletedTask;
        }
    }
}