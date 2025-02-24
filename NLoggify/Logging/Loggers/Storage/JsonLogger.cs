using NLoggify.Logging.Config;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace NLoggify.Logging.Loggers.Storage
{
    /// <summary>
    /// Logger that writes logs in JSON format.
    /// </summary>
    internal class JsonLogger : FileLogger
    {
        public JsonLogger() : base()
        {
            // Change the extension to .json
            _filePath = Path.ChangeExtension(_filePath, "json");
        }

        /// <summary>
        /// Formats the log entry as a JSON object.
        /// </summary>
        #if !DEBUG
        [ExcludeFromCodeCoverage] // No reason to test it
        #endif
        protected override string FormatLog(LogLevel level, string message, string timestamp)
        {
            var logEntry = new
            {
                timestamp,
                level = level.ToString(),
                message
            };

            string logLine = JsonSerializer.Serialize(logEntry);
#if DEBUG
            debugOutputRedirect = logLine;
#endif
            return logLine;
        }
    }
}
