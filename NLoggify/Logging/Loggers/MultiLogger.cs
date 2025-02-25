using NLoggify.Logging.Config;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace NLoggify.Logging.Loggers
{
    /// <summary>
    /// A logger that combines multiple loggers and writes logs to all of them.
    /// </summary>
    internal class MultiLogger : Logger
    {
        private static readonly object _lock = new(); // Lock for configuration safety

        /// <summary>
        /// Initializes the MultiLogger, creating a dedicated thread for each logger.
        /// </summary>
        internal MultiLogger() { }


        [ExcludeFromCodeCoverage] // No reason to test it
        public override void Log(LogLevel level, string message)
        {
            // Execute each iteration in parallel
            Parallel.ForEach(LoggingConfig.Loggers, logger => logger.Log(level, message));
        }

        // No reason to implement it, no reason to test it :P
        [ExcludeFromCodeCoverage]
        protected override void WriteLog(string prefix, string message)
        {
            throw new NotImplementedException();
        }

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
