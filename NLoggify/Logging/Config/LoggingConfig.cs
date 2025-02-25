using NLoggify.Logging.Config.Enums;
using NLoggify.Logging.Loggers;
using NLoggify.Logging.Loggers.Output;
using NLoggify.Logging.Loggers.Storage;
using NLoggify.Utils;
using System.Diagnostics.CodeAnalysis;

namespace NLoggify.Logging.Config
{
    /// <summary>
    /// Represents a global configuration for the logging system.
    /// </summary>
    public partial class LoggingConfig
    {
        internal static List<Logger> Loggers { get; private set; } = GenericUtils.GetEnumValues<LoggerType>()
            .Where(type => type != LoggerType.Multi)
            .ToList()
            .Select(GetLoggerBasedOnType)
            .ToList(); // List of enabled loggers (if multilogging is going on)

        /// <summary>
        /// Gets the current minimum log level required for messages to be recorded.
        /// </summary>
        public LogLevel MinimumLogLevel { get; set; } = LogLevel.Info;

        /// <summary>
        /// Gets the currently configured logger type.
        /// </summary>
        public LoggerType LoggerType { get; set; } = LoggerType.Console;

        /// <summary>
        /// Gets or sets the format used to display timestamps in the log messages.
        /// The default format is "yyyy-MM-dd HH:mm:ss", but it can be changed to any valid DateTime format string.
        /// </summary>
        public string TimestampFormat 
        {
            get => _timestampFormat;
            set { if (ConfigValidation.ValidateTimestampFormat(value)) _timestampFormat = value; }
        }
        private string _timestampFormat = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// Gets or sets addiontal info about threads involved in the logging session
        /// </summary>
        public bool IncludeThreadInfo { get; set; } = Environment.ProcessorCount > 1;

        /// <summary>
        /// Get an instance of <see cref="LoggingConfig"/>, the configuration class for the entire log system
        /// </summary>
        public LoggingConfig()
        {
            FileSection = new FileLoggingConfig(this);
        }

        /// <summary>
        /// Configures logging to multiple destinations (Console, File, Debug, etc.)
        /// </summary>
        /// <param name="loggers">The types of loggers to be used</param>
        [ExcludeFromCodeCoverage] // No reason to test it
        public static void ConfigureMultiLogger(params LoggerType[] loggers)
        {
            _ConfigureMultiLogger(loggers);
        }

        internal static List<Logger> _ConfigureMultiLogger(params LoggerType[] loggers)
        {
            // Clear the loggers list
            Loggers.Clear();

            // Set to track the added logger types
            var addedLoggerTypes = new HashSet<LoggerType>();

            foreach (var loggerType in loggers)
            {
                // Check if the type has already been added
                if (addedLoggerTypes.Contains(loggerType))
                {
                    throw new ArgumentException($"The logger type '{loggerType}' has already been added.");
                }

                // Check if it's trying to adding a MultiLogger
                if (loggerType == LoggerType.Multi)
                {
                    throw new ArgumentException("A MultiLogger cannot be added to the logger configuration.");
                }

                // Add the logger type to the list
                Loggers.Add(GetLoggerBasedOnType(loggerType));
                addedLoggerTypes.Add(loggerType);
            }
            return Loggers;
        }

        /// <summary>
        /// Creates an instance of the logger based on the current configuration settings.
        /// </summary>
        /// <returns>An instance of a logger corresponding to the configured <see cref="LoggerType"/>.</returns>
        internal Logger CreateLogger()
        {
            return GetLoggerBasedOnType(LoggerType);
        }

        /// <summary>
        /// Gets an instance of the logger based on the given <see cref="LoggerType"/>
        /// </summary>
        /// <param name="type">Type of logger</param>
        /// <returns>An instance of the logger based on the given <see cref="LoggerType"/></returns>
        /// <exception cref="NotSupportedException">The logger is not supported</exception>
        internal static Logger GetLoggerBasedOnType(LoggerType type)
        {
            return type switch
            {
                LoggerType.Debug => new DebugLogger(),
                LoggerType.Console => new ConsoleLogger(),
                LoggerType.PlainText => new PlainTextLogger(),
                LoggerType.JSON => new JsonLogger(),
                LoggerType.Multi => new MultiLogger(),
                _ => throw new NotSupportedException("The specified logger type is not supported.")
            };
        }
    }
}