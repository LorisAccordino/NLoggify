using NLoggify.Logging.Config.Enums;
using NLoggify.Utils;

namespace NLoggify.Logging.Config
{
    /// <summary>
    /// Represents a global configuration for the logging system.
    /// </summary>
    public class LoggerConfig : ICloneable
    {
        /// <summary>
        /// Gets the current minimum log level required for messages to be recorded.
        /// </summary>
        public LogLevel MinimumLogLevel { get; set; } = LogLevel.Info;

        /// <summary>
        /// Gets the currently configured logger type.
        /// </summary>
        //public LoggerType LoggerType { get; set; } = LoggerType.Console;

        /// <summary>
        /// Gets or sets the format used to display timestamps in the log messages.
        /// The default format is "yyyy-MM-dd HH:mm:ss", but it can be changed to any valid DateTime format string.
        /// </summary>
        public string TimestampFormat 
        {
            get => timestampFormat;
            set { if (ConfigValidator.ValidateTimestampFormat(value)) timestampFormat = value; }
        }
        private string timestampFormat = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// Gets or sets addiontal info about threads involved in the logging session
        /// </summary>
        public bool IncludeThreadInfo { get; set; } = Environment.ProcessorCount > 1;


        /// <summary>
        /// Gets a deep copy of <see langword="this"/> object
        /// </summary>
        /// <returns>A copy of <see langword="this"/> object</returns>
        public object Clone()
        {
            return GenericUtils.DeepCopy(this);
        }
    }
}