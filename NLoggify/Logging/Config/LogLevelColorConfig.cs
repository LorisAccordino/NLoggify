using NLoggify.Logging.Config.Enums;

namespace NLoggify.Logging.Config
{
    public partial class LoggingConfig
    {
        /// <summary>
        /// Section about the <see cref="Console"/> log level colors
        /// </summary>
        public LogLevelColorConfig ColorsSection { get; }

        /// <summary>
        /// Provides configuration for log level colors in the console.
        /// This class allows you to define and retrieve the color associated with each log level.
        /// </summary>
        public class LogLevelColorConfig
        {
            private LoggingConfig loggingConfig; // Reference to CurrentConfig (it can be useful!)

            /// <summary>
            /// Indicates whether use or not colors in the console logging mode
            /// </summary>
            public bool UseColors { get; set; }

            // Dictionary that maps log levels to console colors
            private readonly Dictionary<LogLevel, ConsoleColor> _logLevelColors = new Dictionary<LogLevel, ConsoleColor>
            {
                { LogLevel.Trace, ConsoleColor.Gray },
                { LogLevel.Debug, ConsoleColor.Green },
                { LogLevel.Info, ConsoleColor.Cyan },
                { LogLevel.Warning, ConsoleColor.Yellow },
                { LogLevel.Error, ConsoleColor.DarkYellow },
                { LogLevel.Critical, ConsoleColor.Red },
                { LogLevel.Fatal, ConsoleColor.DarkRed }
            };

            /// <summary>
            /// Gets the console color associated with the specified log level.
            /// </summary>
            /// <param name="level">The log level for which the color is needed.</param>
            /// <returns>The console color associated with the given log level. Returns <see cref="ConsoleColor.White"/> if the level is not found.</returns>
            internal ConsoleColor GetColorForLevel(LogLevel level)
            {
                return _logLevelColors.ContainsKey(level) ? _logLevelColors[level] : ConsoleColor.White;
            }

            /// <summary>
            /// Allows dynamic configuration of a single log level color.
            /// This method updates the color for the specified log level.
            /// </summary>
            /// <param name="level">The log level to configure.</param>
            /// <param name="color">The console color to associate with the specified log level.</param>
            public void ConfigureLogLevelColor(LogLevel level, ConsoleColor color)
            {
                if (_logLevelColors.ContainsKey(level)) _logLevelColors[level] = color;
            }

            /// <summary>
            /// Allows dynamic configuration of console colors for multiple log levels.
            /// This method allows you to update the default colors or add custom colors for any log level.
            /// </summary>
            /// <param name="customColors">A dictionary mapping log levels to custom console colors.</param>
            /// <remarks>
            /// If a color is provided for an existing log level, it will replace the default color.
            /// If a new log level is provided, it will be added to the configuration.
            /// </remarks>
            public void ConfigureLogLevelColors(Dictionary<LogLevel, ConsoleColor> customColors)
            {
                foreach (var color in customColors)
                {
                    ConfigureLogLevelColor(color.Key, color.Value);
                }
            }

            // Get reference to CurrentConfig
            internal LogLevelColorConfig(LoggingConfig loggingConfig)
            {
                this.loggingConfig = loggingConfig;
            }
        }
    }
}