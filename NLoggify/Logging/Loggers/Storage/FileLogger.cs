﻿using System.Diagnostics.CodeAnalysis;

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
        protected string _filePath = loggingConfig.FileSection.FullPath; // File path copy for local manipulation

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
    }
}
