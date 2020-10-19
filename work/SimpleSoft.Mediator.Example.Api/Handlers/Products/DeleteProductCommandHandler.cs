using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SimpleSoft.Mediator.Example.Api.Database;

namespace SimpleSoft.Mediator.Example.Api.Handlers.Products
{
    public class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand>
    {
        private readonly DbSet<ProductEntity> _products;

        public DeleteProductCommandHandler(ExampleApiContext context)
        {
            _products = context.Set<ProductEntity>();
        }

        public async Task HandleAsync(DeleteProductCommand cmd, CancellationToken ct)
        {
            var entity = await _products.SingleOrDefaultAsync(e => e.ExternalId == cmd.ProductId, ct);
            if (entity == null)
                throw new InvalidOperationException($"Entity with id '{cmd.ProductId}' not found");

            _products.Remove(entity);
        }

        public class Validator : AbstractValidator<DeleteProductCommand>
        {
            public Validator()
            {
                RuleFor(e => e.ProductId)
                    .NotEmpty();
            }
        }
    }
}