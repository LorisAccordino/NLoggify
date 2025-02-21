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
            if (Instance == null)
            {
                // You could initialize the logger here if needed
                throw new InvalidOperationException("Logger has not been configured.");
            }

            return Instance;
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
            if (level < LoggingConfig.LogLevel)
                return;

            // Call the concrete implementation of logging
            WriteLog(level, message);
        }

        /// <summary>
        /// Writes the log message to the target output. Must be implemented by derived classes.
        /// </summary>
        /// <param name="level">The severity level of the log message.</param>
        /// <param name="message">The log message to be recorded.</param>
        protected abstract void WriteLog(LogLevel level, string message);

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