using NLoggify.Logging.Config;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace NLoggify.Logging.Loggers
{
    /// <summary>
    /// Proxy for the logger instance, ensuring that all references always point to the current logger.
    /// This allows dynamic updates to the logger configuration while maintaining a consistent reference.
    /// </summary>
    internal sealed class LoggerProxy : ILogger, IDisposable
    {
        private static readonly LoggerProxy _instance = new LoggerProxy();
        private static readonly object _lock = new object(); // Lock object for thread safety

#if DEBUG
        [ExcludeFromCodeCoverage] // No reason to test it
        public static string GetDebugOutput() { return Logger.GetDebugOutput(); }
#endif

        /// <summary>
        /// Gets the singleton instance of <see cref="LoggerProxy"/>.
        /// </summary>
        public static LoggerProxy Instance => _instance;

        /// <summary>
        /// Gets the current logger instance from <see cref="Logger"/>.
        /// This ensures that if the logger type is changed, all calls are directed to the updated logger.
        /// </summary>
        private Logger CurrentLogger => Logger.Instance;

        /// <summary>
        /// Prevents the client to instance this
        /// </summary>
        private LoggerProxy() { }

        /// <summary>
        /// Logs a message with the specified log level.
        /// </summary>
        /// <param name="level">The log level of the message.</param>
        /// <param name="message">The message to be logged.</param>
#if !DEBUG
        [ExcludeFromCodeCoverage] // No reason to test it
#endif
        public void Log(LogLevel level, string message)
        {
            lock(_lock)
            {
                CurrentLogger.Log(level, message);
            }
        }

        /// <summary>
        /// Logs an exception with a specified log level.
        /// </summary>
        /// <param name="level">The log level for the exception.</param>
        /// <param name="action">The action (that contains a potentially exception) to be executed.</param>
        /// <param name="message">The log message to be recorded.</param>
        /// <returns>True if the exception was thrown, otherwise false</returns>
#if !DEBUG
        [ExcludeFromCodeCoverage] // No reason to test it
#endif
        public bool LogException(LogLevel level, Action action, string message = "")
        {
            return CurrentLogger.LogException(level, action, message);
        }

        /// <summary>
        /// Logs an async exception with a specified log level.
        /// </summary>
        /// <param name="level">The log level for the exception.</param>
        /// <param name="action">The action (that contains a potentially exception) to be executed.</param>
        /// <param name="message">The log message to be recorded.</param>
        /// <returns>True if the exception was thrown, otherwise false</returns>
        public async Task<bool> LogException(LogLevel level, Func<Task> action, string message = "")
        {
            return await CurrentLogger.LogException(level, action, message);
        }
        
        [ExcludeFromCodeCoverage] // No reason to test it
        public void Dispose()
        {
            CurrentLogger.Dispose();
        }
    }
}
