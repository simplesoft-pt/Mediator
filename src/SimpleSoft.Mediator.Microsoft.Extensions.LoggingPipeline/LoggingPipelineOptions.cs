using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace SimpleSoft.Mediator
{
    /// <summary>
    /// Logging pipeline options
    /// </summary>
    public class LoggingPipelineOptions
    {
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
        /// Settings for JSON serialization. Defaults to 'new JsonSerializerSettings(){ Formatting = Formatting.Indented }'.
        /// </summary>
        public JsonSerializerSettings SerializerSettings { get; set; } = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented
        };
    }
}