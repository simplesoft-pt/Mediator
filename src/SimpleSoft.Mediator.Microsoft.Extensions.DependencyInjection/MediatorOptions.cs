using System;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// The mediator registration options
    /// </summary>
    public class MediatorOptions
    {
        /// <summary>
        /// The function that builds the <see cref="IMediatorFactory"/> to be used. By default,
        /// it will build a <see cref="MicrosoftMediatorFactory"/> instance.
        /// </summary>
        public Func<IServiceProvider, IMediatorFactory> FactoryBuilder { get; set; } =
            provider => new MicrosoftMediatorFactory(provider);

        /// <summary>
        /// The lifetime of the <see cref="IMediatorFactory"/> instance. By default,
        /// the value is <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public ServiceLifetime FactoryLifetime { get; set; } = ServiceLifetime.Singleton;

        /// <summary>
        /// The function that builds the <see cref="IMediator"/> to be used. By default,
        /// it will build a <see cref="Mediator"/> instance.
        /// </summary>
        public Func<IServiceProvider, IMediator> MediatorBuilder { get; set; } =
            provider => new Mediator(provider.GetRequiredService<IMediatorFactory>());

        /// <summary>
        /// The lifetime of the <see cref="IMediator"/> instance. By default,
        /// the value is <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public ServiceLifetime MediatorLifetime { get; set; } = ServiceLifetime.Singleton;
    }
}