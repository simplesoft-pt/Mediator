using System;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// The mediator registration options
    /// </summary>
    public class MediatorOptions
    {
        private Func<IServiceProvider, IMediator> _mediatorBuilder =
            provider => new Mediator(provider.GetRequiredService<IMediatorFactory>());

        private Func<IServiceProvider, IMediatorFactory> _factoryBuilder =
            provider => new MicrosoftMediatorFactory(provider);

        /// <summary>
        /// The function that builds the <see cref="IMediatorFactory"/> to be used. By default,
        /// it will build a <see cref="MicrosoftMediatorFactory"/> instance.
        /// </summary>
        public Func<IServiceProvider, IMediatorFactory> FactoryBuilder
        {
            get { return _factoryBuilder; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                _factoryBuilder = value;
            }
        }

        /// <summary>
        /// The lifetime of the <see cref="IMediatorFactory"/> instance. By default,
        /// the value is <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public ServiceLifetime FactoryLifetime { get; set; } = ServiceLifetime.Singleton;

        /// <summary>
        /// The function that builds the <see cref="IMediator"/> to be used. By default,
        /// it will build a <see cref="Mediator"/> instance.
        /// </summary>
        public Func<IServiceProvider, IMediator> MediatorBuilder
        {
            get { return _mediatorBuilder; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                _mediatorBuilder = value;
            }
        }

        /// <summary>
        /// The lifetime of the <see cref="IMediator"/> instance. By default,
        /// the value is <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public ServiceLifetime MediatorLifetime { get; set; } = ServiceLifetime.Singleton;
    }
}