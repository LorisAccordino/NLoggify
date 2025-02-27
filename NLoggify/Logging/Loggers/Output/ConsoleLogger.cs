using NLoggify.Logging.Config;
using NLoggify.Logging.Config.Enums;

namespace NLoggify.Logging.Loggers.Output
{
    /// <summary>
    /// A logger that writes log messages to the console (standard output).
    /// </summary>
    internal class ConsoleLogger : Logger
    {
        private static readonly object localLock = new object(); // Lock object for thread safety

        private readonly ConsoleLoggerConfig config;

        public ConsoleLogger(ConsoleLoggerConfig? config = null) : base(config)
        {
            this.config = config ?? new ConsoleLoggerConfig();
        }

        public override void Log(LogLevel level, string message)
        {
            lock (localLock)
            {
                // Should use colors?
                bool colors = config.UseColors;
                // Change the console color based on the log level
                if (colors) Console.ForegroundColor = config.GetColorForLevel(level);

                // Log as usual
                base.Log(level, message);
                
                // Reset the console color back to the default
                if (colors) Console.ResetColor();
            }
        }

        protected override void WriteLog(string header, string message)
        {
            // Print the log message with the formatted header and message
            string logLine = header + message;
            Console.WriteLine(logLine);
#if DEBUG
            debugOutputRedirect = logLine;
#endif
        }
    }
}