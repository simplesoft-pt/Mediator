using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SimpleSoft.Mediator.Example.Cmd.Pipelines
{
    public class ValidationPipeline : Pipeline
    {
        private readonly ILogger<ValidationPipeline> _logger;
        private readonly IServiceProvider _provider;

        public ValidationPipeline(ILogger<ValidationPipeline> logger, IServiceProvider provider)
        {
            _logger = logger;
            _provider = provider;
        }

        public override async Task OnCommandAsync<TCommand>(Func<TCommand, CancellationToken, Task> next, TCommand cmd, CancellationToken ct)
        {
            await ValidateAsync(cmd, ct);
            await next(cmd, ct);
        }

        public override async Task<TResult> OnCommandAsync<TCommand, TResult>(Func<TCommand, CancellationToken, Task<TResult>> next, TCommand cmd, CancellationToken ct)
        {
            await ValidateAsync(cmd, ct);
            return await next(cmd, ct);
        }

        private async Task ValidateAsync<T>(T cmd, CancellationToken ct)
        {
            var validator = _provider.GetService<IValidator<T>>();
            if (validator == null)
            {
                _logger.LogWarning("Validator for type '{instanceType}' not found", typeof(T).Name);
                return;
            }

            _logger.LogDebug("Validating instance");
            await validator.ValidateAsync(cmd, ct);
        }
    }
}
