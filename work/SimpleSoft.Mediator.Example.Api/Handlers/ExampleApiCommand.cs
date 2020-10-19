using System.Security.Principal;

namespace SimpleSoft.Mediator.Example.Api.Handlers
{
    public abstract class ExampleApiCommand<TResult> : Command<TResult>
    {
        protected ExampleApiCommand(IIdentity identity) => CreatedBy = identity?.Name;
    }

    public abstract class ExampleApiCommand : Command
    {
        protected ExampleApiCommand(IIdentity identity) => CreatedBy = identity?.Name;
    }
}