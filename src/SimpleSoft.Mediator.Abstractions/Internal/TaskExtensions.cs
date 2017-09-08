using System.Threading.Tasks;

namespace SimpleSoft.Mediator.Internal
{
    internal static class TaskExtensions
    {

#if NET40

        public static Task InternalConfigureAwait(this Task task, bool continueOnCapturedContext = false)
        {
            return task;
        }

        public static Task<T> InternalConfigureAwait<T>(this Task<T> task, bool continueOnCapturedContext = false)
        {
            return task;
        }

        public static void InternalWait(this Task task)
        {
            task.Wait();
        }

        public static T InternalWait<T>(this Task<T> task)
        {
            task.Wait();

            return task.Result;
        }

#else

        public static async Task InternalConfigureAwait(this Task task, bool continueOnCapturedContext = false)
        {
            await task.ConfigureAwait(continueOnCapturedContext);
        }

        public static async Task<T> InternalConfigureAwait<T>(this Task<T> task, bool continueOnCapturedContext = false)
        {
            return await task.ConfigureAwait(continueOnCapturedContext);
        }

        public static void InternalWait(this Task task)
        {
            task.ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public static T InternalWait<T>(this Task<T> task)
        {
            return task.ConfigureAwait(false).GetAwaiter().GetResult();
        }

#endif

    }
}
