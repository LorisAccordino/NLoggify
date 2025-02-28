using NLoggify.Logging.Config;
using System.Diagnostics.CodeAnalysis;

namespace NLoggify.Logging.Loggers.Storage
{
    /// <summary>
    /// Logger that writes logs in plain text format.
    /// </summary>
    /// 
    internal class PlainTextLogger : FileLogger
    {
        internal PlainTextLogger() : this(new FileLoggerConfig()) { }
        internal PlainTextLogger(LoggerConfig config) : this(config as FileLoggerConfig ?? new FileLoggerConfig(config)) { }
        internal PlainTextLogger(FileLoggerConfig config) : base(config)
        {
            // Change the extension to .log
            filePath = Path.ChangeExtension(filePath, "log");
        }

        [ExcludeFromCodeCoverage] // No reason to test it
        protected override string FormatLog(string header, string message)
        {
            string logLine = header + message;
#if DEBUG
            debugOutputRedirect = logLine;
#endif
            return logLine;
        }
    }
}
