using System;
using System.Security.Principal;

namespace SimpleSoft.Mediator.Example.Api.Handlers.Products
{
    public class GetProductByIdQuery : ExampleApiQuery<Product>
    {
        public GetProductByIdQuery(IIdentity identity) : base(identity)
        {

        }

        public Guid ProductId { get; set; }
    }
}