﻿using NLoggify.Logging.Config;
using NLoggify.Logging.Config.Enums;
using System.Diagnostics.CodeAnalysis;

namespace NLoggify.Logging.Loggers
{
    /// <summary>
    /// A logger that combines multiple loggers and writes logs to all of them.
    /// </summary>
    internal class MultiLogger : Logger
    {
        //private static readonly object lock = new(); // Lock for configuration safety
        private readonly LoggerConfig config;

        /// <summary>
        /// Initializes the MultiLogger, creating a dedicated thread for each logger.
        /// </summary>
        internal MultiLogger(LoggerConfig config) : base(config)
        {
            this.config = config;
        }


        [ExcludeFromCodeCoverage] // No reason to test it
        public override void Log(LogLevel level, string message)
        {
            // Execute each iteration in parallel
            Parallel.ForEach(config.Loggers, logger => logger.Log(level, message));
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
            foreach (var logger in config.Loggers)
            {
                logger.Dispose();
            }
        }
    }
}
