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
        /// Registers a <see cref="LoggingPipeline"/>.
        /// </summary>
        /// <param name="options">The mediator registration options</param>
        /// <param name="config">Configures the pipeline options</param>
        /// <param name="lifetime">The pipeline lifetime</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static MediatorOptions AddPipelineForLogging(
            this MediatorOptions options, Action<LoggingPipelineOptions> config = null, ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            if (config != null)
                options.Services.Configure(config);

            options.AddPipeline<LoggingPipeline>(lifetime);

            return options;
        }
    }
}
