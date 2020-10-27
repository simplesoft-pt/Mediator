using System;
using System.Linq;
using System.Reflection;
using FluentValidation;
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
        /// <param name="config">Configures the pipeline options</param>
        /// <param name="lifetime">The pipeline lifetime</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static MediatorOptions AddPipelineForValidation(
            this MediatorOptions options, Action<ValidationPipelineOptions> config = null, ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            if (config != null)
                options.Services.Configure(config);

            options.AddPipeline<ValidationPipeline>(lifetime);

            return options;
        }

        /// <summary>
        /// Registers all the <see cref="IValidator{T}"/> found in the given assembly.
        /// </summary>
        /// <param name="options">The mediator options</param>
        /// <param name="assembly">The assembly to scan</param>
        /// <param name="lifetime">The validators lifetime</param>
        /// <returns>The options instance after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static MediatorOptions AddValidatorsFromAssembly(this MediatorOptions options, 
            Assembly assembly, ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
#if NETSTANDARD1_1
            var exportedTypes = assembly.ExportedTypes.Select(t => t.GetTypeInfo());
#else
            var exportedTypes = assembly.ExportedTypes;
#endif
            foreach (var t in exportedTypes.Where(t => t.IsClass && !t.IsAbstract))
            {
#if NETSTANDARD1_1
                var implementedInterfaces = t.ImplementedInterfaces.Select(i => i.GetTypeInfo());
#else
                var implementedInterfaces = t.GetInterfaces();
#endif
                foreach (var i in implementedInterfaces.Where(e => e.IsGenericType && e.GetGenericTypeDefinition() == typeof(IValidator<>)))
                {
#if NETSTANDARD1_1
                    var serviceType = i.AsType();
                    var implementationType = t.AsType();
#else
                    var serviceType = i;
                    var implementationType = t;
#endif
                    options.Services.Add(new ServiceDescriptor(serviceType, implementationType, lifetime));
                }
            }

            return options;
        }

        /// <summary>
        /// Registers all the <see cref="IValidator{T}"/> found in the assembly of the given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options"></param>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        public static MediatorOptions AddValidatorsFromAssemblyOf<T>(this MediatorOptions options, ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
#if NETSTANDARD1_1
            return options.AddValidatorsFromAssembly(typeof(T).GetTypeInfo().Assembly, lifetime);
#else
            return options.AddValidatorsFromAssembly(typeof(T).Assembly, lifetime);
#endif
        }
    }
}
