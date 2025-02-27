using NLoggify.Logging.Config;
using NLoggify.Logging.Config.Enums;

namespace NLoggify.Logging.Loggers.Output
{
    /// <summary>
    /// A logger that writes log messages to the console (standard output).
    /// </summary>
    internal class ConsoleLogger : Logger
    {

        private readonly ConsoleLoggerConfig config;
        private readonly bool useColors;
        private LogLevel messageLogLevel;

        public ConsoleLogger() : this(new ConsoleLoggerConfig()) { }
        public ConsoleLogger(LoggerConfig config) : this(new ConsoleLoggerConfig(config)) { }

        public ConsoleLogger(ConsoleLoggerConfig config) : base(config)
        {
            this.config = config ?? new ConsoleLoggerConfig();
            useColors = this.config.UseColors;
        }


        public override void Log(LogLevel level, string message)
        {
            lock (localLock) messageLogLevel = level;
            base.Log(level, message);
        }

        protected override void WriteLog(string header, string message)
        {
            // Change the console color based on the log level
            if (useColors) Console.ForegroundColor = config.GetColorForLevel(messageLogLevel);

            // Print the log message with the formatted header and message
            string logLine = header + message;
            Console.WriteLine(logLine);

            // Reset the console color back to the default
            if (useColors) Console.ResetColor();

#if DEBUG
            debugOutputRedirect = logLine;
#endif
        }
    }
}