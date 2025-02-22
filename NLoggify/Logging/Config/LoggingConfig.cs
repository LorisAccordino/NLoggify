﻿using NLoggify.Logging.Loggers;
using NLoggify.Logging.Loggers.Output;
using NLoggify.Logging.Loggers.Storage;

namespace NLoggify.Logging.Config
{
    /// <summary>
    /// Represents a global configuration settings for the logging system.
    /// </summary>
    public static class LoggingConfig
    {
        private static readonly object _lock = new object();

        /// <summary>
        /// Gets the current minimum log level required for messages to be recorded.
        /// </summary>
        public static LogLevel MinimumLogLevel { get; private set; } = LogLevel.Info;

        /// <summary>
        /// Gets the currently configured logger type.
        /// </summary>
        public static LoggerType LoggerType { get; private set; } = LoggerType.Console;

        /// <summary>
        /// Gets the file path used for file-based logging (ignored for console logging).
        /// </summary>
        public static string FilePath { get; private set; } = Directory.GetCurrentDirectory() + "/logs/output.log";

        /// <summary>
        /// Gets or sets the format used to display timestamps in the log messages.
        /// The default format is "yyyy-MM-dd HH:mm:ss", but it can be changed to any valid DateTime format string.
        /// </summary>
        public static string TimestampFormat { get; private set; } = "yyyy-MM-dd HH:mm:ss";


        /// <summary>
        /// Updates the logging configuration dynamically at runtime.
        /// </summary>
        /// <param name="minimumLogLevel">The minimum log level required for messages to be recorded.</param>
        /// <param name="loggerType">The type of logger to use.</param>
        /// <param name="filePath">The file path for file-based logging (ignored for console logging).</param>
        /// <param name="timestampFormat">The format to use for logging timestamps (optional).</param>
        public static void Configure(LogLevel minimumLogLevel, LoggerType loggerType, string filePath = "", string timestampFormat = "")
        {
            lock (_lock)
            {
                // Assign log level and type
                MinimumLogLevel = minimumLogLevel;
                LoggerType = loggerType;

                // Validate the path
                if (ConfigValidation.ValidatePath(filePath)) FilePath = filePath;

                // Validate the timestamp format
                if (ConfigValidation.ValidateTimestampFormat(timestampFormat)) TimestampFormat = timestampFormat;

                // Reconfigure the logger
                Logger.Reconfigure();
            }
        }

        /// <summary>
        /// Creates an instance of the logger based on the current configuration settings.
        /// </summary>
        /// <returns>An instance of a logger corresponding to the configured <see cref="LoggerType"/>.</returns>
        internal static Logger CreateLogger()
        {
            return LoggerType switch
            {
                LoggerType.Debug => new DebugLogger(),
                LoggerType.Console => new ConsoleLogger(),
                LoggerType.PlainText => new PlainTextLogger(FilePath),
                LoggerType.JSON => new JsonLogger(FilePath),
                _ => throw new NotSupportedException("The specified logger type is not supported.")
            };
        }
    }
}