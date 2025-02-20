namespace NLoggify.Logging
{
    /// <summary>
    /// Interface defining the behaviour for loggers.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs a message with the specified log level.
        /// </summary>
        /// <param name="level">The log level that categorizes the importance of the message.</param>
        /// <param name="message">The log message to be recorded.</param>
        void Log(LogLevel level, string message);
    }
}