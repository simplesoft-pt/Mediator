using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using SimpleSoft.Mediator.Middleware;

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// Factory for mediator dependencies that build services directly from
    /// the <see cref="IServiceProvider"/> instance.
    /// </summary>
    public class MicrosoftMediatorFactory : IMediatorFactory
    {
        private readonly IServiceProvider _provider;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="provider">The service provider</param>
        /// <exception cref="ArgumentNullException"></exception>
        public MicrosoftMediatorFactory(IServiceProvider provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));

            _provider = provider;
        }

        /// <inheritdoc />
        public ICommandHandler<TCommand> BuildCommandHandlerFor<TCommand>() where TCommand : ICommand
        {
            return _provider.GetService<ICommandHandler<TCommand>>();
        }

        /// <inheritdoc />
        public ICommandHandler<TCommand, TResult> BuildCommandHandlerFor<TCommand, TResult>() where TCommand : ICommand<TResult>
        {
            return _provider.GetService<ICommandHandler<TCommand, TResult>>();
        }

        /// <inheritdoc />
        public IEnumerable<IEventHandler<TEvent>> BuildEventHandlersFor<TEvent>() where TEvent : IEvent
        {
            return _provider.GetServices<IEventHandler<TEvent>>();
        }

        /// <inheritdoc />
        public IQueryHandler<TQuery, TResult> BuildQueryHandlerFor<TQuery, TResult>() where TQuery : IQuery<TResult>
        {
            return _provider.GetService<IQueryHandler<TQuery, TResult>>();
        }

        /// <inheritdoc />
        public IEnumerable<ICommandMiddleware> BuildCommandMiddlewares()
        {
            return _provider.GetServices<ICommandMiddleware>();
        }

        /// <inheritdoc />
        public IEnumerable<IEventMiddleware> BuildEventMiddlewares()
        {
            return _provider.GetServices<IEventMiddleware>();
        }

        /// <inheritdoc />
        public IEnumerable<IQueryMiddleware> BuildQueryMiddlewares()
        {
            return _provider.GetServices<IQueryMiddleware>();
        }
    }
}
