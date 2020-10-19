using System.Security.Principal;

namespace SimpleSoft.Mediator.Example.Api.Handlers.Products
{
    public class CreateProductCommand : ExampleApiCommand<Product>
    {
        public CreateProductCommand(IIdentity identity) : base(identity)
        {

        }

        public string Code { get; set; }

        public string Name { get; set; }
    }
}