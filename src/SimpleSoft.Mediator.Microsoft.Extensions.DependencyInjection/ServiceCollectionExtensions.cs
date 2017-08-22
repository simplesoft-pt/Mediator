using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// Extensions for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configures the mediator into the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="config">An optional configuration delegate</param>
        /// <returns>The service collection after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMediator(
            this IServiceCollection services, Action<MediatorOptions> config = null)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var options = new MediatorOptions();
            config?.Invoke(options);

            services.TryAdd(new ServiceDescriptor(
                typeof(IMediator), options.MediatorBuilder, options.MediatorLifetime));
            services.TryAdd(new ServiceDescriptor(
                typeof(IMediatorFactory), options.FactoryBuilder, options.FactoryLifetime));

            return services;
        }
    }
}
