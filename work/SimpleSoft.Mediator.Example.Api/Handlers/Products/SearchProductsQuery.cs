using System.Security.Principal;

namespace SimpleSoft.Mediator.Example.Api.Handlers.Products
{
    public class SearchProductsQuery : ExampleApiQuery<Page<Product>>
    {
        public SearchProductsQuery(IIdentity identity) : base(identity)
        {

        }

        public string FilterQ { get; set; }

        public int? Skip { get; set; }

        public int? Take { get; set; }
    }
}
