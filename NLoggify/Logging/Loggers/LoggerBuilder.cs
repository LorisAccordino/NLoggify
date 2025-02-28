using NLoggify.Logging.Loggers.Output;
using NLoggify.Logging.Loggers.Storage;
using NLoggify.Logging.Config;
using NLoggify.Logging.Config.Advanced;
using NLoggify.Logging.Config.Enums;

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

        internal LoggerBuilder WriteToLogger<TLogger>(Func<TLogger> createLogger) where TLogger : Logger
        {
            // Check for any duplicates
            if (loggers.Any(logger => logger.GetType() == typeof(TLogger)) && !RiskySettings.AllowMultipleSameLoggers)
                throw new InvalidOperationException($"{typeof(TLogger).Name} is already registered! \n" +
                    "[Warning]: If you need to have multiple loggers pointing on the same output destination (Console, File etc...), " +
                    "you can enable this feature by setting RiskySettings.AllowMultipleSameLoggers = true. At your own risk!");

            loggers.Add(createLogger()); // Add the new logger
            return this;
        }

        /// <summary>
        /// Write log messages to the <see cref="System.Diagnostics.Debug"/> output
        /// </summary>
        /// <param name="config">The optional configuration (the default config will be used if <see langword="null"/>)</param>
        /// <returns></returns>
        public LoggerBuilder WriteToDebug(LoggerConfig? config = null)
        {
            return WriteToLogger(() => new DebugLogger(config));
        }

        /// <summary>
        /// Write log messages to the <see cref="Console"/> output
        /// </summary>
        /// <param name="config">The optional configuration (the default config will be used if <see langword="null"/>)</param>
        /// <returns></returns>
        public LoggerBuilder WriteToConsole(LoggerConfig? config = null)
        {
            return WriteToLogger(() => new ConsoleLogger(config));
        }


        /*
        public LoggerBuilder WriteToConsole(ConsoleLoggerConfig config)
        {
            return WriteToLogger(() => new ConsoleLogger(config));
        }

        public LoggerBuilder WriteToConsole(LoggerConfig config)
        {
            return WriteToConsole(new ConsoleLoggerConfig(config));
        }

        public LoggerBuilder WriteToConsole()
        {
            return WriteToConsole(new LoggerConfig());
        }
        */

        /// <summary>
        /// Write log messages to a <see cref="File"/> output
        /// </summary>
        /// <param name="config">The optional configuration (the default config will be used if <see langword="null"/>)</param>
        /// <returns></returns>
        public LoggerBuilder WriteToPlainTextFile(LoggerConfig? config = null)
        {
            return WriteToLogger(() => new PlainTextLogger(config));
        }

        /// <summary>
        /// Write log messages to a JSON <see cref="File"/> output
        /// </summary>
        /// <param name="config">The optional configuration (the default config will be used if <see langword="null"/>)</param>
        /// <returns></returns>
        public LoggerBuilder WriteToJsonFile(LoggerConfig? config = null)
        {
            return WriteToLogger(() => new JsonLogger(config));
        }



        /*** BUILD ***/

        /// <summary>
        /// Build the loggers system completely configured
        /// </summary>
        public void Build()
        {
            if (loggers.Count == 0)
            {
                if (RiskySettings.AllowDefaultLogger)
                {
                    LoggerWrapper.Instance.Log(LogLevel.Warning, "No logger output configured. Using default ConsoleLogger.");
                    WriteToConsole();
                }
                else
                    throw new InvalidOperationException("Empty loggers list! You must call LoggerBuilder.WriteToXXX() at least once to get a consistent logger");
            }

            ILogger internalLogger = loggers.Count == 1 ? loggers[0] : new MultiLogger(loggers);
            LoggerWrapper.Instance.SetInternalLogger(internalLogger);

            // Mark the logging system as configured
            IsConfigured = true;
        }
    }
}
