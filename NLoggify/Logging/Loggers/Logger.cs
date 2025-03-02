using NLoggify.Logging.Config;
using NLoggify.Logging.Config.Enums;

namespace NLoggify.Logging.Loggers
{
    /// <summary>
    /// Abstract base class for logger representation. The actual implementation of logging (e.g., console, file, etc.) is done in derived classes.
    /// </summary>
    public abstract class Logger : ILogger
    {
        //private static Logger? instance = null;  // Static instance for the Singleton pattern

        /*** SYNCHRONIZATION ***/
        protected readonly object localLock = new object(); // Lock object for thread safety
        private readonly SemaphoreSlim asyncLock = new(1, 1);   // Async lock object for async operations
        protected static readonly object sharedLock = new object(); // Shared lock for complex sync operations
        //protected static readonly object configLock = new object(); // Config logging for config operations
        /***********************/

        private readonly LoggerConfig config;

#if DEBUG
        public static string debugOutputRedirect = ""; // Used for debug
        public static string GetDebugOutput() 
        {
            lock (sharedLock)
            {
                var output = debugOutputRedirect;
                debugOutputRedirect = "";
                return output;
            }
        }
#endif

        internal Logger() : this(new LoggerConfig()) { }

        internal Logger(LoggerConfig config)
        {
            this.config = config ?? new LoggerConfig();
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
            lock (localLock)
            {
                // Filtering logic: Only log messages that meet or exceed the configured level
                if (level < config.MinimumLogLevel)
                    return;


                // Should track threads?
                int threadId = -1;
                string? threadName = "";
                if (config.IncludeThreadInfo)
                {
                    threadId = Thread.CurrentThread.ManagedThreadId;
                    threadName = Thread.CurrentThread.Name;
                }

                // Call the concrete implementation of logging
                string header = GetLogHeader(level, DateTime.Now.ToString(config.TimestampFormat), threadId, threadName);
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
        public virtual bool LogException(LogLevel level, Action action, string message = "")
        {
            lock (localLock)
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
        public virtual async Task<bool> LogException(LogLevel level, Func<Task> action, string message = "")
        {
            await asyncLock.WaitAsync(); // Wait that no threads are executing the method

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
                asyncLock.Release(); // Release the lock for the next thread
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
        /// Writes the log message to the target output. Must be implemented by derived classes.
        /// </summary>
        /// <param name="header">The header to put before the log message.</param>
        /// <param name="message">The log message to be recorded.</param>
        protected virtual void WriteLog(string header, string message) { }


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