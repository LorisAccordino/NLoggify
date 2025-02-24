﻿using NLoggify.Logging.Config;

namespace NLoggify.Logging.Loggers
{
    /// <summary>
    /// A logger that combines multiple loggers and writes logs to all of them.
    /// </summary>
    #if DEBUG
    public class MultiLogger : Logger
    #else
    internal class MultiLogger : Logger
    #endif
    {
        /// <summary>
        /// Logs a message to all configured loggers.
        /// </summary>
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
        public override void Dispose()
        {
            foreach (var logger in LoggingConfig.Loggers)
            {
                logger.Dispose();
            }
        }
    }
}
