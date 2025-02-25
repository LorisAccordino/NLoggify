using NLoggify.Logging.Config;
using NLoggify.Logging.Loggers;
using System.Diagnostics.CodeAnalysis;

namespace NLoggify.Logging.Loggers.Storage
{
    /// <summary>
    /// Base class for file-based loggers, handling file writing operations.
    /// Specific log formats (e.g., PlainText, JSON) should extend this class.
    /// </summary>
    internal abstract class FileLogger : Logger
    {
        //private readonly string _filePath;
        private readonly object _fileLock = new(); // Lock for thread-safe writing
        protected string _filePath = ""; // Local copy of FileLoggingConfig.FilePath for local manipulation

        /// <summary>
        /// Initializes a new instance of the <see cref="FileLogger"/> class.
        /// </summary>
        ///// <param name="filePath">The file path where logs will be written.</param>
        protected FileLogger()
        {
            // File logging configuration
            FileLoggingConfig.EnableTimestampedLogFile();
            _filePath = FileLoggingConfig.FilePath;
            FileLoggingConfig.EnsureLogDirectoryExists();
        }

#if !DEBUG
        [ExcludeFromCodeCoverage] // No reason to test it
#endif
        protected override sealed void WriteLog(string header, string message)
        {
            var logEntry = FormatLog(header, message);

            lock (_fileLock) // Ensure thread-safety when writing to the file
            {
                File.AppendAllText(_filePath, logEntry + Environment.NewLine);
            }
        }

        /// <summary>
        /// Defines how the log entry should be formatted.
        /// Implemented by subclasses.
        /// </summary>
        /// <param name="prefix">The prefix to put before the log message.</param>
        /// <param name="message">The log message.</param>
        /// <returns>The formatted log entry.</returns>
        protected abstract string FormatLog(string prefix, string message);

        [ExcludeFromCodeCoverage] // No reason to test it
        public override void Dispose() { }
    }
}
