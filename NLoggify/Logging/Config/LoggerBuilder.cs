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

        // Indicate whether the logger has been built or not
        private bool isBuilt = false;

        // List of (already configured) loggers to use for log management.
        private List<Logger> loggers = new List<Logger>();


        // Private constructor to prevent direct instantiation of the class
        internal LoggerBuilder() { }


        private LoggerBuilder WriteToLogger<TLogger>(Func<TLogger> createLogger) where TLogger : Logger
        {
            // Check for any duplicates
            if (loggers.Any(logger => logger.GetType() == typeof(TLogger)))
                throw new InvalidOperationException($"{typeof(TLogger).Name} is already registered!");

            loggers.Add(createLogger()); // Add the new logger
            return this;
        }

        /// <summary> Write log messages to the <see cref="System.Diagnostics.Debug"/> output  </summary>
        /// <param name="config">The optional configuration (the default config will be used if <see langword="null"/>)</param>
        public LoggerBuilder WriteToDebug(LoggerConfig? config = null) => WriteToLogger(() => new DebugLogger(config));

        /// <summary>  Write log messages to the <see cref="Console"/> output </summary>
        /// <param name="config">The optional configuration (the default config will be used if <see langword="null"/>)</param>
        public LoggerBuilder WriteToConsole(LoggerConfig? config = null) => WriteToLogger(() => new ConsoleLogger(config));

        /// <summary> Write log messages to a <see cref="File"/> output </summary>
        /// <param name="config">The optional configuration (the default config will be used if <see langword="null"/>)</param>
        public LoggerBuilder WriteToPlainTextFile(LoggerConfig? config = null) => WriteToLogger(() => new PlainTextLogger(config));

        /// <summary> Write log messages to a JSON <see cref="File"/> output </summary>
        /// <param name="config">The optional configuration (the default config will be used if <see langword="null"/>)</param>
        public LoggerBuilder WriteToJsonFile(LoggerConfig? config = null) => WriteToLogger(() => new JsonLogger(config));



        /// <summary>
        /// Build the loggers system completely configured
        /// </summary>
        public ILogger Build()
        {
            if (loggers.Count == 0) throw new InvalidOperationException("Logger configuration started but no output specified.");
            if (isBuilt) throw new InvalidOperationException("Logger already built! You cannot reconfigure or rebuild the logger");

            ILogger loggerInstance = loggers.Count == 1 ? loggers[0] : new MultiLogger(loggers);
            //LoggerWrapper.Instance.SetInternalLogger(internalLogger);
            LoggerManager.SetLogger(loggerInstance);

            // Mark as built
            isBuilt = true;
            return loggerInstance;
        }

        /// <summary> Default logger (<see cref="Console"/> output) with default configuration </summary>
        public LoggerBuilder Default()
        {
            DefaultBuildWarning();
            return WriteToConsole();
        }

        // Show a warning to notify the user about the default configuration
        private static void DefaultBuildWarning()
        {
            if (SuppressWarnings) return; // Skip the warning

            Console.WriteLine("********************************************************************************");
            Console.WriteLine("|  WARNING: Logger not configured. Using default ConsoleLogger.                |");
            Console.WriteLine("|  You can suppress warnings by setting LoggerBuilder.SuppressWarnings = true  |");
            Console.WriteLine("********************************************************************************");
        }
    }
}
