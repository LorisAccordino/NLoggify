using NLoggify.Logging.Loggers.Output;
using NLoggify.Logging.Loggers.Storage;
using NLoggify.Logging.Config.Advanced;
using NLoggify.Logging.Loggers;

namespace NLoggify.Logging.Config
{
    /// <summary>
    /// Configuration builder for the logging system. It uses the "Builder" pattern to enable the creation
    /// and configuration of a logging sytsem in a seamlessly and controlled manner.
    /// </summary>
    public class LoggerBuilder
    {
        /// <summary>
        /// Indicate whether suppress logger configuration/building warnings or not
        /// </summary>
        public static bool SuppressWarnings { get; set; } = false;

        /// <summary>
        /// Default logger (with default configuration), in case of no configuration
        /// </summary>
        /// <returns></returns>
        public static LoggerBuilder Default() => new LoggerBuilder().WriteToConsole();

        // Singleton instance
        private static LoggerBuilder Instance => instance;
        private static readonly LoggerBuilder instance = new LoggerBuilder();

        // Flags to indicate the configuration and building state
        internal static bool IsConfigured { get; private set; } = false;
        internal static bool IsBuilt { get; private set; } = false;

        // List of (already configured) loggers to use for log management.
        private List<Logger> loggers = new List<Logger>();

        // Private constructor to prevent direct instantiation of the class
        internal LoggerBuilder() { }

        /// <summary>
        /// Get the <see cref="LoggerBuilder"/> reference to configure the logging system
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Cannot reconfigure the logging system, unless <see cref="RiskySettings.AllowReconfiguration"/> = <see langword="true"/></exception>
        public static LoggerBuilder Configure()
        {
            if (IsConfigured && !RiskySettings.AllowReconfiguration)
                throw new InvalidOperationException("Logger already configured! Use Logger.GetLogger() to get the logger reference. \n" +
                    "[Warning]: It is not reccommended to hot-reload the logging system with reconfiguration at runtime, but, if you really need it, \n" +
                    "you can enable this feature by setting RiskySettings.AllowReconfiguration = true. At your own risk!");

            // Mark as configured
            IsConfigured = true;
            return Instance;
        }


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
        public LoggerBuilder WriteToDebug(LoggerConfig? config = null) => WriteToLogger(() => new DebugLogger(config));

        /// <summary>
        /// Write log messages to the <see cref="Console"/> output
        /// </summary>
        /// <param name="config">The optional configuration (the default config will be used if <see langword="null"/>)</param>
        /// <returns></returns>
        public LoggerBuilder WriteToConsole(LoggerConfig? config = null) => WriteToLogger(() => new ConsoleLogger(config));

        /// <summary>
        /// Write log messages to a <see cref="File"/> output
        /// </summary>
        /// <param name="config">The optional configuration (the default config will be used if <see langword="null"/>)</param>
        /// <returns></returns>
        public LoggerBuilder WriteToPlainTextFile(LoggerConfig? config = null) => WriteToLogger(() => new PlainTextLogger(config));

        /// <summary>
        /// Write log messages to a JSON <see cref="File"/> output
        /// </summary>
        /// <param name="config">The optional configuration (the default config will be used if <see langword="null"/>)</param>
        /// <returns></returns>
        public LoggerBuilder WriteToJsonFile(LoggerConfig? config = null) => WriteToLogger(() => new JsonLogger(config));



        /*** BUILD ***/

        /// <summary>
        /// Build the loggers system completely configured
        /// </summary>
        public void Build()
        {
            if (loggers.Count == 0) throw new InvalidOperationException("Logger configuration started but no output specified.");
            /*if (loggers.Count == 0)
            {
                if (RiskySettings.AllowDefaultLogger)
                {
                    LoggerWrapper.Instance.Log(LogLevel.Warning, "No logger output configured. Using default ConsoleLogger.");
                    WriteToConsole();
                }
                else
                    throw new InvalidOperationException("Empty loggers list! You must call LoggerBuilder.WriteToXXX() at least once to get a consistent logger");
            }*/

            ILogger internalLogger = loggers.Count == 1 ? loggers[0] : new MultiLogger(loggers);
            LoggerWrapper.Instance.SetInternalLogger(internalLogger);

            // Mark as built
            IsBuilt = true;
        }
    }
}
