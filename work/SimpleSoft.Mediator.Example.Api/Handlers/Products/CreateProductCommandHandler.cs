using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SimpleSoft.Mediator.Example.Api.Database;

namespace SimpleSoft.Mediator.Example.Api.Handlers.Products
{
    public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Product>
    {
        private readonly DbSet<ProductEntity> _products;

        public CreateProductCommandHandler(ExampleApiContext context)
        {
            _products = context.Set<ProductEntity>();
        }

        public async Task<Product> HandleAsync(CreateProductCommand cmd, CancellationToken ct)
        {
            if (await _products.AnyAsync(e => e.Code.Equals(cmd.Code, StringComparison.OrdinalIgnoreCase), ct))
                throw new InvalidOperationException($"Duplicated product code '{cmd.Code}'");

            var entity = (await _products.AddAsync(new ProductEntity
            {
                ExternalId = Guid.NewGuid(),
                Code = cmd.Code.ToLowerInvariant(),
                Name = cmd.Name
            }, ct)).Entity;

            return new Product
            {
                Id = entity.ExternalId,
                Code = entity.Code,
                Name = entity.Name
            };
        }

        public class Validator : AbstractValidator<CreateProductCommand>
        {
            public Validator()
            {
                RuleFor(e => e.Code)
                    .NotEmpty()
                    .Length(8)
                    .Matches("^[0-9a-zA-Z]*$");

                RuleFor(e => e.Name)
                    .NotEmpty()
                    .MaximumLength(128);
            }
        }
    }
}