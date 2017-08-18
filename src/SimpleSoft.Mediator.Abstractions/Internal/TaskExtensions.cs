using System.Threading.Tasks;

namespace SimpleSoft.Mediator.Internal
{
    internal static class TaskExtensions
    {
        public static readonly Task CompletedTask = Task.FromResult(true);
    }
}
