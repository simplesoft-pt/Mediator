using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// Handler factory that uses delegates to build the required services
    /// </summary>
    public class DelegateHandlerFactory : IHandlerFactory
    {
        private readonly Service _serviceFactory;
        private readonly CollectionService _collectionServiceFactory;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="serviceFactory"></param>
        /// <param name="collectionServiceFactory"></param>
        public DelegateHandlerFactory(Service serviceFactory, CollectionService collectionServiceFactory)
        {
            if (serviceFactory == null) throw new ArgumentNullException(nameof(serviceFactory));
            if (collectionServiceFactory == null) throw new ArgumentNullException(nameof(collectionServiceFactory));

            _serviceFactory = serviceFactory;
            _collectionServiceFactory = collectionServiceFactory;
        }

        /// <inheritdoc />
        public ICommandHandler<TCommand> BuildCommandHandlerFor<TCommand>() 
            where TCommand : ICommand
        {
            var service = _serviceFactory(typeof(ICommandHandler<TCommand>));

            return (ICommandHandler<TCommand>) service;
        }

        /// <inheritdoc />
        public ICommandHandler<TCommand, TResult> BuildCommandHandlerFor<TCommand, TResult>()
            where TCommand : ICommand<TResult>
        {
            var service = _serviceFactory(typeof(ICommandHandler<TCommand, TResult>));

            return (ICommandHandler<TCommand, TResult>) service;
        }

        /// <inheritdoc />
        public IEnumerable<IEventHandler<TEvent>> BuildEventHandlersFor<TEvent>() 
            where TEvent : IEvent
        {
            var services = _collectionServiceFactory(typeof(IEventHandler<TEvent>));

            return services?.Cast<IEventHandler<TEvent>>() ?? Enumerable.Empty<IEventHandler<TEvent>>();
        }

        /// <summary>
        /// Delegate used to resolve handler services.
        /// </summary>
        /// <param name="serviceType">The service type</param>
        /// <returns>The resolved instance</returns>
        public delegate object Service(Type serviceType);

        /// <summary>
        /// Delegate used to resolve a collection of services.
        /// </summary>
        /// <param name="serviceType">The service type</param>
        /// <returns>The collection of resolved instances</returns>
        public delegate IEnumerable<object> CollectionService(Type serviceType);
    }
}
