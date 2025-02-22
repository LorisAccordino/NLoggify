using NLoggify.Logging.Config;

namespace NLoggify.Logging.Loggers.Storage
{
    /// <summary>
    /// Logger that writes logs in plain text format.
    /// </summary>
    internal class PlainTextLogger : FileLogger
    {
        /// <summary>
        /// Formats the log entry as plain text.
        /// </summary>
        protected override string FormatLog(LogLevel level, string message, string timestamp)
        {
            return $"[{timestamp}] [{level}] {message}";
        }
    }
}
