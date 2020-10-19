using System;
using System.Security.Principal;

namespace SimpleSoft.Mediator.Example.Api.Handlers.Products
{
    public class DeleteProductCommand : ExampleApiCommand
    {
        public DeleteProductCommand(IIdentity identity) : base(identity)
        {

        }

        public Guid ProductId { get; set; }
    }
}