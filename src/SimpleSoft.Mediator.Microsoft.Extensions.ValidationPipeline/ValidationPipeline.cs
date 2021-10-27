﻿using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// Validation pipeline
    /// </summary>
    public class ValidationPipeline : IPipeline
    {
        private readonly IServiceProvider _provider;
        private readonly ValidationPipelineOptions _options;
        private readonly ILogger<ValidationPipeline> _logger;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public ValidationPipeline(
            IServiceProvider provider, 
            IOptions<ValidationPipelineOptions> options, 
            ILogger<ValidationPipeline> logger)
        {
            _provider = provider;
            _options = options.Value;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task OnCommandAsync<TCommand>(Func<TCommand, CancellationToken, Task> next, TCommand cmd, CancellationToken ct) 
            where TCommand : class, ICommand
        {
            if (_options.ValidateCommand)
                await ValidateInstanceAsync(cmd, ct).ConfigureAwait(false);
            await next(cmd, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<TResult> OnCommandAsync<TCommand, TResult>(Func<TCommand, CancellationToken, Task<TResult>> next, TCommand cmd, CancellationToken ct) 
            where TCommand : class, ICommand<TResult>
        {
            if (_options.ValidateCommand)
                await ValidateInstanceAsync(cmd, ct).ConfigureAwait(false);
            return await next(cmd, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task OnEventAsync<TEvent>(Func<TEvent, CancellationToken, Task> next, TEvent evt, CancellationToken ct) 
            where TEvent : class, IEvent
        {
            if (_options.ValidateEvent)
                await ValidateInstanceAsync(evt, ct).ConfigureAwait(false);
            await next(evt, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<TResult> OnQueryAsync<TQuery, TResult>(Func<TQuery, CancellationToken, Task<TResult>> next, TQuery query, CancellationToken ct) 
            where TQuery : class, IQuery<TResult>
        {
            if (_options.ValidateQuery)
                await ValidateInstanceAsync(query, ct).ConfigureAwait(false);
            return await next(query, ct).ConfigureAwait(false);
        }

        private async Task ValidateInstanceAsync<T>(T instance, CancellationToken ct) where  T : class
        {
            var validator = _provider.GetService<IValidator<T>>();
            if (validator == null)
            {
                if (_options.FailIfValidatorNotFound)
                    throw new InvalidOperationException($"Validator for '{typeof(T).FullName}' not found");
                
                return;
            }

            _logger.LogDebug("Validating instance");
            var result = await validator.ValidateAsync(instance, ct).ConfigureAwait(false);

            if (result.IsValid)
            {
                _logger.LogInformation("Instance is valid");
                return;
            }

            if (_options.OnFailedValidation != null)
                await _options.OnFailedValidation(instance, result, ct).ConfigureAwait(false);

            throw new ValidationException(result.Errors);
        }
    }
}
