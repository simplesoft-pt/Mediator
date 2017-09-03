#region License
// The MIT License (MIT)
// 
// Copyright (c) 2017 Simplesoft.pt
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
using SimpleSoft.Mediator;
using SimpleSoft.Mediator.Pipeline;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// The mediator registration options
    /// </summary>
    public partial class MediatorOptions
    {
        #region ICommandMiddleware

        private readonly List<ServiceDescriptor> _commandMiddlewareDescriptors = new List<ServiceDescriptor>();
        
        /// <summary>
        /// The collection of <see cref="ICommandMiddleware"/> descriptors
        /// </summary>
        public IReadOnlyCollection<ServiceDescriptor> CommandMiddlewareDescriptors => _commandMiddlewareDescriptors;

        /// <summary>
        /// Clears all registered <see cref="ICommandMiddleware"/>
        /// </summary>
        public MediatorOptions ClearMiddlewareForCommands()
        {
            _commandMiddlewareDescriptors.Clear();
            return this;
        }

        /// <summary>
        /// Registers the given instance as a <see cref="ICommandMiddleware"/>.
        /// </summary>
        /// <param name="instance">The instance to register</param>
        /// <returns>The mediator options after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public MediatorOptions AddMiddlewareForCommands(ICommandMiddleware instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            _commandMiddlewareDescriptors.Add(new ServiceDescriptor(Constants.CommandMiddlewareType, instance));
            return this;
        }

        /// <summary>
        /// Registers the given type as an implementation for <see cref="ICommandMiddleware"/> instances.
        /// </summary>
        /// <typeparam name="TMiddleware">The middleware type</typeparam>
        /// <param name="lifetime">The middleware lifetime</param>
        /// <returns>The mediator options after changes</returns>
        public MediatorOptions AddMiddlewareForCommands<TMiddleware>(ServiceLifetime lifetime = Constants.DefaultMiddlewareLifetime)
            where TMiddleware : ICommandMiddleware
        {
            _commandMiddlewareDescriptors.Add(new ServiceDescriptor(Constants.CommandMiddlewareType, typeof(TMiddleware), lifetime));
            return this;
        }

        /// <summary>
        /// Registers the given function as a factory of <see cref="ICommandMiddleware"/> instances.
        /// </summary>
        /// <param name="factory">The factory to use</param>
        /// <param name="lifetime">The instance lifetime</param>
        /// <returns>The mediator options after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public MediatorOptions AddMiddlewareForCommands(
            Func<IServiceProvider, ICommandMiddleware> factory, ServiceLifetime lifetime = Constants.DefaultMiddlewareLifetime)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            _commandMiddlewareDescriptors.Add(new ServiceDescriptor(Constants.CommandMiddlewareType, factory, lifetime));
            return this;
        }

        #endregion

        #region IEventMiddleware

        private readonly List<ServiceDescriptor> _eventMiddlewareDescriptors = new List<ServiceDescriptor>();

        /// <summary>
        /// The collection of <see cref="IEventMiddleware"/> descriptors
        /// </summary>
        public IReadOnlyCollection<ServiceDescriptor> EventMiddlewareDescriptors => _eventMiddlewareDescriptors;

        /// <summary>
        /// Clears all registered <see cref="IEventMiddleware"/>
        /// </summary>
        public MediatorOptions ClearMiddlewareForEvents()
        {
            _eventMiddlewareDescriptors.Clear();
            return this;
        }

        /// <summary>
        /// Registers the given instance as a <see cref="IEventMiddleware"/>.
        /// </summary>
        /// <param name="instance">The instance to register</param>
        /// <returns>The mediator options after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public MediatorOptions AddMiddlewareForEvents(IEventMiddleware instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            _eventMiddlewareDescriptors.Add(new ServiceDescriptor(Constants.EventMiddlewareType, instance));
            return this;
        }

        /// <summary>
        /// Registers the given type as an implementation for <see cref="IEventMiddleware"/> instances.
        /// </summary>
        /// <typeparam name="TMiddleware">The middleware type</typeparam>
        /// <param name="lifetime">The middleware lifetime</param>
        /// <returns>The mediator options after changes</returns>
        public MediatorOptions AddMiddlewareForEvents<TMiddleware>(ServiceLifetime lifetime = Constants.DefaultMiddlewareLifetime)
            where TMiddleware : IEventMiddleware
        {
            _eventMiddlewareDescriptors.Add(new ServiceDescriptor(Constants.EventMiddlewareType, typeof(TMiddleware), lifetime));
            return this;
        }

        /// <summary>
        /// Registers the given function as a factory of <see cref="IEventMiddleware"/> instances.
        /// </summary>
        /// <param name="factory">The factory to use</param>
        /// <param name="lifetime">The instance lifetime</param>
        /// <returns>The mediator options after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public MediatorOptions AddMiddlewareForEvents(
            Func<IServiceProvider, IEventMiddleware> factory, ServiceLifetime lifetime = Constants.DefaultMiddlewareLifetime)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            _eventMiddlewareDescriptors.Add(new ServiceDescriptor(Constants.EventMiddlewareType, factory, lifetime));
            return this;
        }

        #endregion
        
        #region IEventMiddleware

        private readonly List<ServiceDescriptor> _queryMiddlewareDescriptors = new List<ServiceDescriptor>();

        /// <summary>
        /// The collection of <see cref="IQueryMiddleware"/> descriptors
        /// </summary>
        public IReadOnlyCollection<ServiceDescriptor> QueryMiddlewareDescriptors => _queryMiddlewareDescriptors;

        /// <summary>
        /// Clears all registered <see cref="IQueryMiddleware"/>
        /// </summary>
        public MediatorOptions ClearMiddlewareForQueries()
        {
            _queryMiddlewareDescriptors.Clear();
            return this;
        }

        /// <summary>
        /// Registers the given instance as a <see cref="IQueryMiddleware"/>.
        /// </summary>
        /// <param name="instance">The instance to register</param>
        /// <returns>The mediator options after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public MediatorOptions AddMiddlewareForQueries(IQueryMiddleware instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            _queryMiddlewareDescriptors.Add(new ServiceDescriptor(Constants.QueryMiddlewareType, instance));
            return this;
        }

        /// <summary>
        /// Registers the given type as an implementation for <see cref="IQueryMiddleware"/> instances.
        /// </summary>
        /// <typeparam name="TMiddleware">The middleware type</typeparam>
        /// <param name="lifetime">The middleware lifetime</param>
        /// <returns>The mediator options after changes</returns>
        public MediatorOptions AddMiddlewareForQueries<TMiddleware>(ServiceLifetime lifetime = Constants.DefaultMiddlewareLifetime)
            where TMiddleware : IQueryMiddleware
        {
            _queryMiddlewareDescriptors.Add(new ServiceDescriptor(Constants.QueryMiddlewareType, typeof(TMiddleware), lifetime));
            return this;
        }

        /// <summary>
        /// Registers the given function as a factory of <see cref="IQueryMiddleware"/> instances.
        /// </summary>
        /// <param name="factory">The factory to use</param>
        /// <param name="lifetime">The instance lifetime</param>
        /// <returns>The mediator options after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public MediatorOptions AddMiddlewareForQueries(
            Func<IServiceProvider, IQueryMiddleware> factory, ServiceLifetime lifetime = Constants.DefaultMiddlewareLifetime)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            _queryMiddlewareDescriptors.Add(new ServiceDescriptor(Constants.QueryMiddlewareType, factory, lifetime));
            return this;
        }

        #endregion

        #region IMiddleware

        /// <summary>
        /// Clears all registered middlewares.
        /// </summary>
        public MediatorOptions ClearMiddleware()
        {
            ClearMiddlewareForCommands();
            ClearMiddlewareForEvents();
            ClearMiddlewareForQueries();

            return this;
        }

        /// <summary>
        /// Registers the given instance as a <see cref="ICommandMiddleware"/>,
        /// <see cref="IEventMiddleware"/> and <see cref="IQueryMiddleware"/>.
        /// </summary>
        /// <param name="instance">The instance to register</param>
        /// <returns>The mediator options after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public MediatorOptions AddMiddleware(IMiddleware instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            AddMiddlewareForCommands(instance);
            AddMiddlewareForEvents(instance);
            AddMiddlewareForQueries(instance);

            return this;
        }

        /// <summary>
        /// Registers the given type as an implementation for <see cref="ICommandMiddleware"/>,
        /// <see cref="IEventMiddleware"/> and <see cref="IQueryMiddleware"/> instances.
        /// </summary>
        /// <typeparam name="TMiddleware">The middleware type</typeparam>
        /// <param name="lifetime">The middleware lifetime</param>
        /// <returns>The mediator options after changes</returns>
        public MediatorOptions AddMiddleware<TMiddleware>(ServiceLifetime lifetime = Constants.DefaultMiddlewareLifetime)
            where TMiddleware : IMiddleware
        {
            AddMiddlewareForCommands<TMiddleware>(lifetime);
            AddMiddlewareForEvents<TMiddleware>(lifetime);
            AddMiddlewareForQueries<TMiddleware>(lifetime);

            return this;
        }

        /// <summary>
        /// Registers the given function as a factory of <see cref="ICommandMiddleware"/>,
        /// <see cref="IEventMiddleware"/> and <see cref="IQueryMiddleware"/> instances.
        /// </summary>
        /// <param name="factory">The factory to use</param>
        /// <param name="lifetime">The instance lifetime</param>
        /// <returns>The mediator options after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public MediatorOptions AddMiddleware(
            Func<IServiceProvider, IMiddleware> factory, ServiceLifetime lifetime = Constants.DefaultMiddlewareLifetime)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            AddMiddlewareForCommands(factory, lifetime);
            AddMiddlewareForEvents(factory, lifetime);
            AddMiddlewareForQueries(factory, lifetime);

            return this;
        }

        #endregion
    }
}