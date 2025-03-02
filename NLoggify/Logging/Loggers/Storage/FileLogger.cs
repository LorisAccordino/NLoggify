using NLoggify.Logging.Config;

namespace NLoggify.Logging.Loggers.Storage
{
    /// <summary>
    /// Base class for file-based loggers, handling file writing operations.
    /// Specific log formats (e.g., PlainText, JSON) should extend this class.
    /// </summary>
    internal abstract class FileLogger : Logger
    {
        private readonly object fileLock = new(); // Lock for thread-safe writing
        protected string filePath = ""; // File path copy for local manipulation

        private readonly FileLoggerConfig config;

        protected FileLogger() : this(new FileLoggerConfig()) { }
        protected FileLogger(LoggerConfig config) : this(new FileLoggerConfig(config)) { }
        protected FileLogger(FileLoggerConfig config) : base(config)
        {
            this.config = config ?? new FileLoggerConfig();
            filePath = this.config.FullPath;

            // Ensure directory does exist
            ConfigValidator.EnsureDirectoryExists(filePath);
        }

#if !DEBUG
        [ExcludeFromCodeCoverage] // No reason to test it
#endif
        protected override sealed void WriteLog(string header, string message)
        {
            var logEntry = FormatLog(header, message);

            lock (fileLock) // Ensure thread-safety when writing to the file
            {
                File.AppendAllText(filePath, logEntry + Environment.NewLine);
            }
        }

        /// <summary>
        /// Defines how the log entry should be formatted.
        /// Implemented by subclasses.
        /// </summary>
        /// <param name="header">The header to put before the log message.</param>
        /// <param name="message">The log message.</param>
        /// <returns>The formatted log entry.</returns>
        protected abstract string FormatLog(string header, string message);
    }
}
