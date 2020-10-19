using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SimpleSoft.Mediator.Example.Api.Database;

namespace SimpleSoft.Mediator.Example.Api.Handlers.Products
{
    public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, Product>
    {
        private readonly DbSet<ProductEntity> _products;

        public GetProductByIdQueryHandler(ExampleApiContext context)
        {
            _products = context.Set<ProductEntity>();
        }

        public async Task<Product> HandleAsync(GetProductByIdQuery query, CancellationToken ct)
        {
            var entity = await _products.SingleOrDefaultAsync(e => e.ExternalId == query.ProductId, ct);
            if (entity == null)
                throw new InvalidOperationException($"Entity with id '{query.ProductId}' not found");

            return new Product
            {
                Id = entity.ExternalId,
                Code = entity.Code,
                Name = entity.Name
            };
        }
    }
}