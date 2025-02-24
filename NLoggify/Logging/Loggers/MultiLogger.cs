using NLoggify.Logging.Config;
using System.Diagnostics.CodeAnalysis;

namespace NLoggify.Logging.Loggers
{
    /// <summary>
    /// A logger that combines multiple loggers and writes logs to all of them.
    /// </summary>
    internal class MultiLogger : Logger
    {
        /// <summary>
        /// Logs a message to all configured loggers.
        /// </summary>
        [ExcludeFromCodeCoverage] // No reason to test it
        protected override void WriteLog(LogLevel level, string message, string timestamp)
        {
            foreach (var logger in LoggingConfig.Loggers)
            {
                logger.Log(level, message);
            }
        }

        /// <summary>
        /// Disposes all loggers in the composite.
        /// </summary>
        [ExcludeFromCodeCoverage] // No reason to test it
        public override void Dispose()
        {
            foreach (var logger in LoggingConfig.Loggers)
            {
                logger.Dispose();
            }
        }
    }
}
