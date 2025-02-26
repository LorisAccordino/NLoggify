using NLoggify.Logging.Config;
using NLoggify.Logging.Config.Enums;

namespace NLoggify.Logging.Loggers
{
    /// <summary>
    /// Abstract base class for logger representation. The actual implementation of logging (e.g., console, file, etc.) is done in derived classes.
    /// </summary>
    public abstract class Logger : ILogger
    {
        private static Logger? _instance = null;  // Static instance for the Singleton pattern
        private readonly object _lock = new object();            // Lock object for thread safety
        private static readonly SemaphoreSlim _asyncLock = new(1, 1);   // Async lock object for async operations
        private static readonly object _masterLock = new object();      // Master lock for complex operations
        private static readonly object _configLock = new object();      // Config logging for configuration phases

        protected static LoggingConfig LoggingConfig { get; private set; } = new LoggingConfig(); // Initialize yet to avoid problems in derived classes
        private static bool hasBeenConfigured = false;
        //private static LoggingConfig _loggingConfig = null; // Dummy object to avoid inconsistency in derived classes

#if DEBUG
        public static string debugOutputRedirect = ""; // Used for debug
        public static string GetDebugOutput() 
        {
            lock (_masterLock)
            {
                var output = debugOutputRedirect;
                debugOutputRedirect = "";
                return output;
            }
        }
#endif

        /// <summary>
        /// Gets the singleton instance of the Logger.
        /// </summary>
#if !DEBUG
        [ExcludeFromCodeCoverage] // No reason to test it
#endif
        internal static Logger Instance
        {
            get
            {
                // Ensure that the instance is created only once, and in a thread-safe manner
                lock (_masterLock)
                {
                    if (_instance == null)
                    {
                        // Concrete classes should initialize the logger instance
                        _instance = LoggingConfig.CreateLogger();
                    }
                    return _instance;
                }
            }
#if DEBUG
            set { _instance = value; }
#endif
        }


        /// <summary>
        /// Executes the given action with exclusive access to the logger.
        /// No other logging operations will be allowed while this executes.
        /// </summary>
        /// <param name="action">The action to execute exclusively.</param>
        public static void RunExclusive(Action action)
        {
            lock (_masterLock)
            {
                action();
            }
        }

        /// <summary>
        /// Executes the given function with exclusive access to the logger.
        /// Ensures no other logging operations run concurrently.
        /// </summary>
        /// <typeparam name="T">The return type of the function.</typeparam>
        /// <param name="func">The function to execute exclusively.</param>
        /// <returns>The result of the executed function.</returns>
        public static T RunExclusive<T>(Func<T> func)
        {
            lock (_masterLock)
            {
                return func();
            }
        }


        /// <summary>
        /// Gets the singleton instance of the logger. This has to be used in the entire logging system.
        /// </summary>
        /// <param name="config">The <see cref="Config.LoggingConfig"/> object that represents the logging configuration<br></br>
        /// If it is <b>null</b>, it will be used the <b>default</b> (or the <b>current</b>, if already set) logging configuration
        /// </param>
        /// <returns>Logger instance.</returns>
        public static ILogger GetLogger(LoggingConfig? config = null)
        {
            lock (_masterLock)
            {
                if (config != null)
                {
                    if (hasBeenConfigured == false)
                    {
                        LoggingConfig = config;
                        hasBeenConfigured = true;
                    }
                    else
                        throw new InvalidOperationException("Cannot cannot change the logger configuration at runtime! \n You could use Logger.Reconfigure(), but it is NOT recommended to change logging config at runtime.");
                }

                //Reconfigure(null); // Force a reconfiguration

                // Set up singleton instances
                _instance = LoggingConfig.CreateLogger();
                return LoggerProxy.Instance;
            }
        }

        /// <summary>
        /// Forces a reconfiguration of the logger, creating a new instance if settings have changed.
        /// </summary>
        /// <param name="config">The <see cref="Config.LoggingConfig"/> object that represents the logging configuration</param>
        [Obsolete("It is not recommended to change the configuration at runtime. Do it only in extreme situations!", false)]
        internal static void Reconfigure(LoggingConfig config)
        {
            lock (_configLock)
            {
                if (config == null) return;

                // Reset configuration state (and configuration)
                hasBeenConfigured = false; // Necessary to not throw exception in Logger.GetLogger();
                LoggingConfig = config;

                // Get a new logger config
                GetLogger(config);
            }
        }

        /// <summary>
        /// Get the log header to put before the log message. Should be overrided to have a custom behaviour
        /// </summary>
        /// /// <param name="level">The severity level of the log message.</param>
        /// <param name="timestamp">The time when the messaged was logged. This allows for accurate logging based on the exact time of logging.</param>
        /// <param name="threadId">The id of the calling thread</param>
        /// <param name="threadName">The name of the calling thread</param>
        /// <returns>The formatted log header</returns>
        protected virtual string GetLogHeader(LogLevel level, string timestamp, int threadId = -1, string? threadName = null)
        {
            // Handle thread info
            string threadInfo = "";
            if (threadId != -1)
            {
                threadInfo = $"[Thread {threadId}";
                threadInfo += !string.IsNullOrEmpty(threadName) ? $" ({threadName})] " : "] ";
            }

            // Return the entire header: timestamp, (threadspec), level
            return $"[{timestamp}] {threadInfo}{level}: ";
        }


        /// <summary>
        /// Logs a message with the specified log level.
        /// </summary>
        /// <param name="level">The log level that categorizes the importance of the message.</param>
        /// <param name="message">The log message to be recorded.</param>
#if !DEBUG
        [ExcludeFromCodeCoverage] // No reason to test it
#endif
        public virtual void Log(LogLevel level, string message)
        {
            lock (_lock)
            {
                // Filtering logic: Only log messages that meet or exceed the configured level
                if (level < LoggingConfig.MinimumLogLevel)
                    return;


                // Should track threads?
                int threadId = -1;
                string? threadName = "";
                if (LoggingConfig.IncludeThreadInfo)
                {
                    threadId = Thread.CurrentThread.ManagedThreadId;
                    threadName = Thread.CurrentThread.Name;
                }

                // Call the concrete implementation of logging
                string header = GetLogHeader(level, DateTime.Now.ToString(LoggingConfig.TimestampFormat), threadId, threadName);
                WriteLog(header, message);
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
            lock (_lock)
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
            await _asyncLock.WaitAsync(); // Wait that no threads are executing the method

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
            finally
            {
                _asyncLock.Release(); // Release the lock for the next thread
            }
        }

        /// <summary>
        /// Writes the log message to the target output. Must be implemented by derived classes.
        /// </summary>
        /// <param name="header">The header to put before the log message.</param>
        /// <param name="message">The log message to be recorded.</param>
        protected abstract void WriteLog(string header, string message);


        /// <summary>
        /// Releases all resources used by the <see cref="Logger"/> class.
        /// </summary>
        /// <remarks>
        /// This method is used to release any unmanaged resources or perform cleanup tasks when the logger
        /// is no longer needed. It should be called explicitly or automatically via a using block or finalizer.
        /// </remarks>
        public virtual void Dispose() { }
    }
}