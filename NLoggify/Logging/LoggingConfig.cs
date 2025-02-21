using NLoggify.Logging.Loggers;

namespace NLoggify.Logging
{
    /// <summary>
    /// Represents a global configuration settings for the logging system.
    /// </summary>
    internal static class LoggingConfig
    {
        private static readonly object _lock = new object();

        /// <summary>
        /// Gets the current minimum log level required for messages to be recorded.
        /// </summary>
        public static LogLevel LogLevel { get; private set; } = LogLevel.Info;

        /// <summary>
        /// Gets the currently configured logger type.
        /// </summary>
        public static LoggerType LoggerType { get; private set; } = LoggerType.Console;

        /// <summary>
        /// Gets the file path used for file-based logging (ignored for console logging).
        /// </summary>
        public static string FilePath { get; private set; } = "log.txt";


        /// <summary>
        /// Updates the logging configuration dynamically at runtime.
        /// </summary>
        /// <param name="logLevel">The minimum log level required for messages to be recorded.</param>
        /// <param name="loggerType">The type of logger to use.</param>
        /// <param name="filePath">The file path for file-based logging (ignored for console logging).</param>
        public static void Configure(LogLevel logLevel, LoggerType loggerType, string? filePath = null)
        {
            lock (_lock)
            {
                LogLevel = logLevel;
                LoggerType = loggerType;
                FilePath = filePath ?? "log.txt";
            }

            // Reconfigure the logger
            Logger.Reconfigure();
        }

        /// <summary>
        /// Creates an instance of the logger based on the current configuration settings.
        /// </summary>
        /// <returns>An instance of a logger corresponding to the configured <see cref="LoggerType"/>.</returns>
        public static Logger CreateLogger()
        {
            /*
            return LoggerType switch
            {

                LoggerType.Console => new ConsoleLogger(),
                LoggerType.PlainText => new PlainTextLogger(FilePath),
                LoggerType.JSON => new JsonLogger(FilePath),
                _ => throw new NotSupportedException("The specified logger type is not supported.")
            };
            */
            return null;
        }
    }
}