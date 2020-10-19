using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimpleSoft.Mediator.Example.Api.Handlers;
using SimpleSoft.Mediator.Example.Api.Handlers.Products;

namespace SimpleSoft.Mediator.Example.Api.Controllers.Products
{
    [Route("products")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<PageModel<ProductModel>> SearchAsync([FromQuery] string filterQ, [FromQuery] int? skip, [FromQuery] int? take, CancellationToken ct)
        {
            var result = await _mediator.FetchAsync<SearchProductsQuery, Page<Product>>(
                new SearchProductsQuery(User.Identity)
                {
                    FilterQ = filterQ,
                    Skip = skip,
                    Take = take
                }, ct);

            return new PageModel<ProductModel>
            {
                Total = result.Total,
                Items = result.Items.Select(e => new ProductModel
                {
                    Id = e.Id,
                    Code = e.Code,
                    Name = e.Name
                })
            };
        }

        [HttpGet("{id:guid}")]
        public async Task<ProductModel> GetByIdAsync([FromRoute] Guid id, CancellationToken ct)
        {
            var result = await _mediator.FetchAsync<GetProductByIdQuery, Product>(
                new GetProductByIdQuery(User.Identity)
                {
                    ProductId = id
                }, ct);

            return new ProductModel
            {
                Id = result.Id,
                Code = result.Code,
                Name = result.Name
            };
        }

        [HttpPost]
        public async Task<ProductModel> CreateAsync([FromBody] CreateProductModel model, CancellationToken ct)
        {
            var result = await _mediator.SendAsync<CreateProductCommand, Product>(
                new CreateProductCommand(User.Identity)
                {
                    Code = model.Code,
                    Name = model.Name
                }, ct);

            return new ProductModel
            {
                Id = result.Id,
                Code = result.Code,
                Name = result.Name
            };
        }

        [HttpDelete("{id:guid}")]
        public async Task DeleteAsync([FromRoute] Guid id, CancellationToken ct)
        {
            await _mediator.SendAsync(new DeleteProductCommand(User.Identity)
            {
                ProductId = id
            }, ct);
        }
    }
}
