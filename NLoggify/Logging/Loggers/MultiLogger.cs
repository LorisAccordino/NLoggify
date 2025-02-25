using NLoggify.Logging.Config;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Emit;

namespace NLoggify.Logging.Loggers
{
    /// <summary>
    /// A logger that combines multiple loggers and writes logs to all of them.
    /// </summary>
    internal class MultiLogger : Logger
    {
        /*
        [ExcludeFromCodeCoverage] // No reason to test it
        protected override void WriteLog(string prefix, string message)
        {
            foreach (var logger in LoggingConfig.Loggers)
            {
                logger.Log(level, message);
            }
        }
        */
        [ExcludeFromCodeCoverage] // No reason to test it
        public override void Log(LogLevel level, string message)
        {
            //base.Log(level, message);
            foreach (var logger in LoggingConfig.Loggers)
            {
                logger.Log(level, message);
            }
        }

        // No reason to implement it, no reason to test it :P
        [ExcludeFromCodeCoverage]
        protected override void WriteLog(string prefix, string message)
        {
            throw new NotImplementedException();
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
