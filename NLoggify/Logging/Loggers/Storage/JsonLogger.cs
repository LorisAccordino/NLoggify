using NLoggify.Logging.Config;
using NLoggify.Logging.Config.Enums;
using System.Text.Json;

namespace NLoggify.Logging.Loggers.Storage
{
    /// <summary>
    /// Logger that writes logs in JSON format.
    /// </summary>
    internal class JsonLogger : FileLogger
    {
        internal JsonLogger() : this(new FileLoggerConfig()) { }
        internal JsonLogger(LoggerConfig config) : this(config as FileLoggerConfig ?? new FileLoggerConfig(config)) { }
        internal JsonLogger(FileLoggerConfig config) : base(config)
        {
            // Change the extension to .json
            filePath = Path.ChangeExtension(filePath, "json");
        }

        protected override string GetLogHeader(LogLevel level, string timestamp, int threadId = -1, string? threadName = null)
        {
            var logHeader = new Dictionary<string, object>
            {
                { "timestamp", timestamp },
                { "level", level.ToString() }
            };

            if (threadId != -1)
            {
                logHeader["threadInfo"] = new { threadId, threadName };
            }

            // Header formatted
            return JsonSerializer.Serialize(logHeader);
        }

#if !DEBUG
        [ExcludeFromCodeCoverage] // No reason to test it
#endif
        protected override string FormatLog(string header, string message)
        {
            var logEntry = new
            {
                header = JsonDocument.Parse(header).RootElement, // Parse the JSON to avoid double serialization
                message,
            };

            string logLine = JsonSerializer.Serialize(logEntry);
#if DEBUG
            debugOutputRedirect = logLine;
#endif
            return logLine;
        }
    }
}
