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
        private ILogger internalLogger;
        //private static readonly object lock = new object(); // Lock object for thread safety

        /// <summary>
        /// Gets the singleton instance of <see cref="LoggerWrapper"/>.
        /// </summary>
        public static LoggerWrapper Instance => instance;
        private static readonly LoggerWrapper instance = new LoggerWrapper();

        // Gets the internal log to wrap
        internal LoggerWrapper() { }

        internal void SetInternalLogger(ILogger internalLogger)
        {
            this.internalLogger = internalLogger;
        }

#if !DEBUG
        [ExcludeFromCodeCoverage] // No reason to test it
#endif
        public override void Log(LogLevel level, string message)
        {
            lock(sharedLock)
            {
                internalLogger.Log(level, message);
            }
        }

        /*
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
        */
        
        [ExcludeFromCodeCoverage] // No reason to test it
        public override void Dispose() => internalLogger.Dispose();
    }
}
