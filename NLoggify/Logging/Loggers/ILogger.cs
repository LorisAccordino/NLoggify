using NLoggify.Logging.Config.Enums;

namespace NLoggify.Logging.Loggers
{
    /// <summary>
    /// Interface defining the behaviour for loggers.
    /// </summary>
    public interface ILogger : IDisposable
    {
#if DEBUG
        public abstract string GetDebugOutput(); // Used for debug
#endif

        /// <summary>
        /// Logs a message with the specified log level.
        /// </summary>
        /// <param name="level">The log level that categorizes the importance of the message.</param>
        /// <param name="message">The log message to be recorded.</param>
        void Log(LogLevel level, string message);

        /// <summary>
        /// Logs an exception with a specified log level.
        /// </summary>
        /// <param name="level">The log level for the exception.</param>
        /// <param name="action">The action (that contains a potentially exception) to be executed.</param>
        /// <param name="message">The log message to be recorded.</param>
        /// <returns>True if the exception was thrown, otherwise false</returns>
        bool LogException(LogLevel level, Action action, string message = "");
    }
}