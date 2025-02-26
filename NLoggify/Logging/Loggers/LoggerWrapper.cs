using NLoggify.Logging.Config.Enums;
using System.Diagnostics.CodeAnalysis;

namespace NLoggify.Logging.Loggers
{
    /// <summary>
    /// Wrapper for the logger instance, ensuring that all references always point to the current logger.
    /// This allows dynamic updates to the logger configuration while maintaining a consistent reference.
    /// </summary>
    internal sealed class LoggerWrapper : Logger
    {
        private static readonly LoggerWrapper _instance = new LoggerWrapper();
        //private static readonly object _lock = new object(); // Lock object for thread safety

        /// <summary>
        /// Gets the singleton instance of <see cref="LoggerWrapper"/>.
        /// </summary>
        public new static LoggerWrapper Instance => _instance;

        /// <summary>
        /// Gets the current logger instance from <see cref="Logger"/>.
        /// This ensures that if the logger type is changed, all calls are directed to the updated logger.
        /// </summary>
        private Logger CurrentLogger => Logger.Instance;

        /// <summary>
        /// Prevents the client to instance this
        /// </summary>
        private LoggerWrapper() { }

#if !DEBUG
        [ExcludeFromCodeCoverage] // No reason to test it
#endif
        public override void Log(LogLevel level, string message)
        {
            lock(_masterLock)
            {
                lock(_configLock)
                {
                    CurrentLogger.Log(level, message);
                }
            }
        }

#if !DEBUG
        [ExcludeFromCodeCoverage] // No reason to test it
#endif
        public override bool LogException(LogLevel level, Action action, string message = "")
        {
            return CurrentLogger.LogException(level, action, message);
        }

        public override async Task<bool> LogException(LogLevel level, Func<Task> action, string message = "")
        {
            return await CurrentLogger.LogException(level, action, message);
        }
        
        [ExcludeFromCodeCoverage] // No reason to test it
        public override void Dispose()
        {
            CurrentLogger.Dispose();
        }
    }
}
