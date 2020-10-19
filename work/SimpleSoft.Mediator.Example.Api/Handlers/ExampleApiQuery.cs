using System.Security.Principal;

namespace SimpleSoft.Mediator.Example.Api.Handlers
{
    public abstract class ExampleApiQuery<TResult> : Query<TResult>
    {
        protected ExampleApiQuery(IIdentity identity) => CreatedBy = identity?.Name;
    }
}
