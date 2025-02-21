namespace NLoggify.Logging.Loggers
{
    /// <summary>
    /// Proxy for the logger instance, ensuring that all references always point to the current logger.
    /// </summary>
    public sealed class LoggerProxy : ILogger, IDisposable
    {
        /// <summary>
        /// Gets the current logger instance.
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
