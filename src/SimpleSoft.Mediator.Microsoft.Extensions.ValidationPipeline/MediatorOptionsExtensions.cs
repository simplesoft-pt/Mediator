using System;
using SimpleSoft.Mediator;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions for <see cref="MediatorOptions"/>.
    /// </summary>
    public static class MediatorOptionsExtensions
    {
        /// <summary>
        /// Registers a <see cref="ValidationPipeline"/>.
        /// </summary>
        /// <param name="options">The mediator registration options</param>
        /// <param name="config">Configures the validation pipeline</param>
        /// <param name="lifetime">The pipeline lifetime</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static MediatorOptions AddPipelineForValidation(
            this MediatorOptions options, Action<ValidationPipelineOptions> config = null, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            if (config != null)
                options.Services.Configure(config);

            options.AddPipeline<ValidationPipeline>(lifetime);

            return options;
        }
    }
}
