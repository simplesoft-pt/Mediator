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
        /// Registers a <see cref="DatabaseTransactionPipeline"/>.
        /// </summary>
        /// <param name="options">The mediator registration options</param>
        /// <param name="config">Configures the pipeline options</param>
        /// <param name="lifetime">The pipeline lifetime</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static MediatorOptions AddPipelineForDatabaseTransaction(
            this MediatorOptions options, Action<DatabaseTransactionPipelineOptions> config = null,
            ServiceLifetime lifetime = ServiceLifetime.Transient
        )
        {
            if (config != null)
                options.Services.Configure(config);

            options.AddPipeline<DatabaseTransactionPipeline>(lifetime);

            return options;
        }
    }
}
