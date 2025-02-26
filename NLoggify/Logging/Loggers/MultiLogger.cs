using NLoggify.Logging.Config.Enums;
using System.Diagnostics.CodeAnalysis;

namespace NLoggify.Logging.Loggers
{
    /// <summary>
    /// A logger that combines multiple loggers and writes logs to all of them.
    /// </summary>
    internal class MultiLogger : Logger
    {
        /// <summary>
        /// Initializes the MultiLogger, creating a dedicated thread for each logger.
        /// </summary>
        internal MultiLogger() : base() { }


        [ExcludeFromCodeCoverage] // No reason to test it
        public override void Log(LogLevel level, string message)
        {
            // Execute each iteration in parallel
            Parallel.ForEach(CurrentConfig.Loggers, logger => logger.Log(level, message));
        }

        [ExcludeFromCodeCoverage] // No reason to test it
        public override void Dispose()
        {
            foreach (var logger in CurrentConfig.Loggers)
            {
                logger.Dispose();
            }
        }
    }
}
