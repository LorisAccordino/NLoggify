using NLoggify.Logging.Config;

namespace NLoggify.Logging.Loggers
{
    /// <summary>
    /// Abstract base class for logger representation. The actual implementation of logging (e.g., console, file, etc.) is done in derived classes.
    /// </summary>
    public abstract class Logger : ILogger, IDisposable
    {
        private static Logger? _instance;  // Static instance for the Singleton pattern
        private static readonly object _lock = new object();  // Lock object for thread safety

        /// <summary>
        /// Gets the singleton instance of the Logger.
        /// </summary>
        internal static Logger Instance
        {
            get
            {
                // Ensure that the instance is created only once, and in a thread-safe manner
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        // Concrete classes should initialize the logger instance
                        _instance = LoggingConfig.CreateLogger();
                    }
                    return _instance;
                }
            }
        }

        /// <summary>
        /// Gets the singleton instance of the logger. This has to be used in the entire logging system.
        /// </summary>
        /// <returns>Logger instance.</returns>
        public static ILogger GetLogger()
        {
            return LoggerProxy.Instance;
        }

        /// <summary>
        /// Forces a reconfiguration of the logger, creating a new instance if settings have changed.
        /// </summary>
        internal static void Reconfigure()
        {
            lock (_lock)
            {
                _instance = LoggingConfig.CreateLogger();
            }
        }

        /// <summary>
        /// Logs a message with the specified log level.
        /// </summary>
        /// <param name="level">The log level that categorizes the importance of the message.</param>
        /// <param name="message">The log message to be recorded.</param>
        public void Log(LogLevel level, string message)
        {
            // Filtering logic: Only log messages that meet or exceed the configured level
            if (level < LoggingConfig.MinimumLogLevel)
                return;

            // Call the concrete implementation of logging
            WriteLog(level, message, DateTime.Now.ToString(LoggingConfig.TimestampFormat));
        }

        /// <summary>
        /// Logs an exception with a specified log level.
        /// </summary>
        /// <param name="level">The log level for the exception.</param>
        /// <param name="action">The action (that contains a potentially exception) to be executed.</param>
        /// <param name="message">The log message to be recorded.</param>
        /// <returns>True if the exception was thrown, otherwise false</returns>
        public bool LogException(LogLevel level, Action action, string message = "")
        {
            // Try to execute the given code
            try
            {
                action();
                return false;
            }
            catch (Exception ex)
            {
                // Log the raised exception
                Log(level, $"{message}\nException: {ex.Message}\n{ex.StackTrace}");
                return true;
            }
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
            try
            {
                await action(); // Await the async operation
                return false; // No exception caught
            }
            catch (Exception ex)
            {
                // Log the raised exception
                Log(level, $"{message}\nException: {ex.Message}\n{ex.StackTrace}");
                return true;
            }
        }

        /// <summary>
        /// Writes the log message to the target output. Must be implemented by derived classes.
        /// </summary>
        /// <param name="level">The severity level of the log message.</param>
        /// <param name="message">The log message to be recorded.</param>
        /// <param name="timestamp">The time when the messaged was logged. This allows for accurate logging based on the exact time of logging.</param>
        protected abstract void WriteLog(LogLevel level, string message, string timestamp);

        /// <summary>
        /// Releases all resources used by the <see cref="Logger"/> class.
        /// </summary>
        /// <remarks>
        /// This method is used to release any unmanaged resources or perform cleanup tasks when the logger
        /// is no longer needed. It should be called explicitly or automatically via a using block or finalizer.
        /// </remarks>
        public abstract void Dispose();
    }
}