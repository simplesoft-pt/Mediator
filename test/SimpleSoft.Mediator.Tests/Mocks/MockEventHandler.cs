using System.Threading;
using System.Threading.Tasks;

namespace SimpleSoft.Mediator.Tests.Mocks
{
    public class MockEventHandler : IEventHandler<MockEvent>
    {
        public Task HandleAsync(MockEvent evt, CancellationToken ct)
        {
            return Task.CompletedTask;
        }
    }
}