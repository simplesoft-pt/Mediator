using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SimpleSoft.Mediator.Example.Api.Database;

namespace SimpleSoft.Mediator.Example.Api.Handlers.Products
{
    public class SearchProductsQueryHandler : IQueryHandler<SearchProductsQuery, Page<Product>>
    {
        private readonly IQueryable<ProductEntity> _products;

        public SearchProductsQueryHandler(ExampleApiContext context)
        {
            _products = context.Set<ProductEntity>();
        }

        public async Task<Page<Product>> HandleAsync(SearchProductsQuery query, CancellationToken ct)
        {
            var filterQuery = _products;

            if (!string.IsNullOrWhiteSpace(query.FilterQ))
            {
                filterQuery = filterQuery
                    .Where(e => e.Code.Contains(query.FilterQ) || e.Name.Contains(query.FilterQ));
            }

            var total = await filterQuery.CountAsync(ct);

            IEnumerable<Product> items;
            if (total == 0) 
                items = Enumerable.Empty<Product>();
            else
            {
                var skip = query.Skip ?? 0;
                var take = query.Take ?? 20;

                items = await filterQuery
                    .OrderBy(e => e.Code)
                    .Skip(skip)
                    .Take(take)
                    .Select(e => new Product
                    {
                        Id = e.ExternalId,
                        Code = e.Code,
                        Name = e.Name
                    })
                    .ToListAsync(ct);
            }

            return new Page<Product>
            {
                Total = total,
                Items = items
            };
        }
    }
}