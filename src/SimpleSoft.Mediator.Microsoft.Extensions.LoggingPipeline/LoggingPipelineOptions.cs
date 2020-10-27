using System;
using Microsoft.Extensions.Logging;

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// Logging pipeline options
    /// </summary>
    public class LoggingPipelineOptions
    {
        private Func<object, string> _serializer;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public LoggingPipelineOptions()
        {
#if NETSTANDARD1_1
            var cachedSettings = new Newtonsoft.Json.JsonSerializerSettings
            {
                Formatting = Newtonsoft.Json.Formatting.Indented
            };
            _serializer = instance => Newtonsoft.Json.JsonConvert.SerializeObject(instance, cachedSettings);
#else
            var cachedSettings = new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true
            };
            _serializer = instance => System.Text.Json.JsonSerializer.Serialize(instance, cachedSettings);
#endif

        }

        /// <summary>
        /// Serialize and log commands? Defaults to 'true'.
        /// </summary>
        public bool LogCommand { get; set; } = true;

        /// <summary>
        /// Serialize and log the command result? Defaults to 'false'.
        /// </summary>
        public bool LogCommandResult { get; set; } = false;

        /// <summary>
        /// Serialize and log events? Defaults to 'true'.
        /// </summary>
        public bool LogEvent { get; set; } = true;

        /// <summary>
        /// Serialize and log queries? Defaults to 'true'.
        /// </summary>
        public bool LogQuery { get; set; } = true;

        /// <summary>
        /// Serialize and log the query result? Defaults to 'false'.
        /// </summary>
        public bool LogQueryResult { get; set; } = false;

        /// <summary>
        /// Log level to be used. Defaults to <see cref="LogLevel.Trace"/>.
        /// </summary>
        public LogLevel Level { get; set; } = LogLevel.Trace;

        /// <summary>
        /// Used to serialize the instance into the logs as a string. Defaults to an indented JSON representation.
        /// </summary>
        public Func<object, string> Serializer
        {
            get => _serializer;
            set => _serializer = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}