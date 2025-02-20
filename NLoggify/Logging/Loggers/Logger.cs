namespace NLoggify.Logging
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
        public static Logger Instance
        {
            get
            {
                // Ensure that the instance is created only once, and in a thread-safe manner
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        // Concrete classes should initialize the logger instance
                        _instance = GetLogger();
                    }
                    return _instance;
                }
            }
        }

        /// <summary>
        /// Method to get a concrete logger instance. This should be implemented by derived classes.
        /// </summary>
        /// <returns>The appropriate Logger instance.</returns>
        protected static Logger GetLogger()
        {
            // Default implementation can return null or throw an exception
            // Since the derived classes must implement it
            throw new NotImplementedException("Derived class must implement GetLogger.");
        }

        /// <summary>
        /// Logs a message with the specified log level.
        /// </summary>
        /// <param name="level">The log level that categorizes the importance of the message.</param>
        /// <param name="message">The log message to be recorded.</param>
        public abstract void Log(LogLevel level, string message);

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