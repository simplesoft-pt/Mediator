using System.Threading.Tasks;

namespace SimpleSoft.Mediator.Internal
{
    internal static class Helpers
    {
        public static readonly Task CompletedTask = Task.FromResult(true);
    }
}
