﻿using NLoggify.Logging.Loggers.Output;
using NLoggify.Logging.Loggers.Storage;
using NLoggify.Logging.Config;
using NLoggify.Logging.Config.Advanced;

namespace NLoggify.Logging.Loggers
{
    /// <summary>
    /// Configuration builder for the logging system. It uses the "Builder" pattern to enable the creation
    /// and configuration of a logging sytsem in a seamlessly and controlled manner.
    /// </summary>
    public class LoggerBuilder
    {
        // Singleton instance
        internal static LoggerBuilder Instance => instance;
        private static readonly LoggerBuilder instance = new LoggerBuilder();

        // Flag to indicate whether the logger has been configured or not
        internal static bool IsConfigured { get; private set; } = false;

        // List of (already configured) loggers to use for log management.
        private List<Logger> loggers = new List<Logger>();

        // Private constructor to prevent direct instantiation of the class
        internal LoggerBuilder() { }


        /*** LOGGER OUTPUTS ***/

        private LoggerBuilder WriteToLogger<TLogger>(Func<TLogger> createLogger) where TLogger : Logger
        {
            // Check for any duplicates
            if (loggers.Any(logger => logger.GetType() == typeof(TLogger)) && !RiskySettings.AllowMultipleSameLoggers)
                throw new InvalidOperationException($"{typeof(TLogger).Name} is already registered! \n" +
                    "[Warning]: If you need to have multiple loggers pointing on the same output destination (Console, File etc...), \n" +
                    "you can enable this feature by setting RiskySettings.AllowMultipleSameLoggers = true. At your own risk!");

            loggers.Add(createLogger()); // Add the new logger
            return this;
        }

        public LoggerBuilder WriteToDebug(LoggerConfig? config = null)
        {
            return WriteToLogger(() => new DebugLogger(config ?? new LoggerConfig()));
        }

        public LoggerBuilder WriteToConsole(ConsoleLoggerConfig? config = null)
        {
            return WriteToLogger(() => new ConsoleLogger(config ?? new ConsoleLoggerConfig()));
        }

        public LoggerBuilder WriteToPlainTextFile(FileLoggerConfig? config = null)
        {
            return WriteToLogger(() => new PlainTextLogger(config ?? new FileLoggerConfig()));
        }

        public LoggerBuilder WriteToJsonFile(FileLoggerConfig? config = null)
        {
            return WriteToLogger(() => new JsonLogger(config ?? new FileLoggerConfig()));
        }



        /*** BUILD ***/

        /// <summary>
        /// Build the loggers system completely configured
        /// </summary>
        public void Build()
        {
            if (loggers.Count == 0) throw new InvalidOperationException("Empty loggers list! You must call LoggerBuilder.WriteToXXX() at least once to get a consistent logger");

            ILogger internalLogger = loggers.Count == 1 ? loggers[0] : new MultiLogger(loggers);
            LoggerWrapper.Instance.SetInternalLogger(internalLogger);

            // Mark the logging system as configured
            IsConfigured = true;
        }
    }
}
