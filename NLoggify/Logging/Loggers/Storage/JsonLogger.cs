using NLoggify.Logging.Config;
using System.Text.Json;

namespace NLoggify.Logging.Loggers.Storage
{
    /// <summary>
    /// Logger that writes logs in JSON format.
    /// </summary>
    internal class JsonLogger : FileLogger
    {
        public JsonLogger(string filePath) : base(filePath) { }

        /// <summary>
        /// Formats the log entry as a JSON object.
        /// </summary>
        protected override string FormatLog(LogLevel level, string message, string timestamp)
        {
            var logEntry = new
            {
                timestamp,
                level = level.ToString(),
                message
            };

            return JsonSerializer.Serialize(logEntry);
        }
    }
}
