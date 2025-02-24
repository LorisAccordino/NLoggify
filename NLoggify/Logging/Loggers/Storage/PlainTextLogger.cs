using NLoggify.Logging.Config;
using System.Diagnostics.CodeAnalysis;

namespace NLoggify.Logging.Loggers.Storage
{
    /// <summary>
    /// Logger that writes logs in plain text format.
    /// </summary>
    #if DEBUG
    public class PlainTextLogger : FileLogger
    #else
    internal class PlainTextLogger : FileLogger
    #endif
    {
        public PlainTextLogger() : base()
        {
            // Change the extension to .log
            _filePath = Path.ChangeExtension(_filePath, "log");
        }

        /// <summary>
        /// Formats the log entry as plain text.
        /// </summary>
        [ExcludeFromCodeCoverage] // No reason to test it
        protected override string FormatLog(LogLevel level, string message, string timestamp)
        {
            string logLine = $"[{timestamp}] {level}: {message}";
            #if DEBUG
            debugOutputRedirect = logLine;
            #endif
            return logLine;
        }
    }
}
