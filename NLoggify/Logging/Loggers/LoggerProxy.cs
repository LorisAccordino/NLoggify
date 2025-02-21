﻿namespace NLoggify.Logging.Loggers
{
    /// <summary>
    /// Proxy for the logger instance, ensuring that all references always point to the current logger.
    /// This allows dynamic updates to the logger configuration while maintaining a consistent reference.
    /// </summary>
    internal sealed class LoggerProxy : ILogger, IDisposable
    {
        private static readonly LoggerProxy _instance = new LoggerProxy();

        /// <summary>
        /// Gets the singleton instance of <see cref="LoggerProxy"/>.
        /// </summary>
        public static LoggerProxy Instance => _instance;

        /// <summary>
        /// Prevents direct instantiation of the <see cref="LoggerProxy"/> class.
        /// </summary>
        private LoggerProxy() { }

        /// <summary>
        /// Gets the current logger instance from <see cref="Logger"/>.
        /// This ensures that if the logger type is changed, all calls are directed to the updated logger.
        /// </summary>
        private Logger CurrentLogger => Logger.Instance;

        /// <summary>
        /// Logs a message with the specified log level.
        /// </summary>
        /// <param name="level">The log level of the message.</param>
        /// <param name="message">The message to be logged.</param>
        public void Log(LogLevel level, string message)
        {
            CurrentLogger.Log(level, message);
        }

        /// <summary>
        /// Releases resources held by the current logger.
        /// </summary>
        public void Dispose()
        {
            CurrentLogger.Dispose();
        }
    }
}
