using System;
using Microsoft.EntityFrameworkCore;
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
        /// Registers a <see cref="EFCoreTransactionPipeline{TDbContext}"/>.
        /// </summary>
        /// <param name="options">The mediator registration options</param>
        /// <param name="config">Configures the pipeline options</param>
        /// <param name="lifetime">The pipeline lifetime</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static MediatorOptions AddPipelineForEFCoreTransaction<TDbContext>(
            this MediatorOptions options, Action<EFCoreTransactionPipelineOptions> config = null, ServiceLifetime lifetime = ServiceLifetime.Scoped) 
            where TDbContext : DbContext
        {
            if (config != null)
                options.Services.Configure(config);

            options.AddPipeline<EFCoreTransactionPipeline<TDbContext>>(lifetime);

            return options;
        }
    }
}
