using NLoggify.Logging.Config.Enums;

namespace NLoggify.Logging.Loggers.Output
{
    /// <summary>
    /// A logger that writes log messages to the console (standard output).
    /// </summary>
    internal class ConsoleLogger : Logger
    {
        private static readonly object _lock = new object(); // Lock object for thread safety

        internal ConsoleLogger() : base() { }

        public override void Log(LogLevel level, string message)
        {
            lock (_lock)
            {
                // Should use colors?
                bool colors = LoggingConfig.ColorsSection.UseColors;
                // Change the console color based on the log level
                if (colors) Console.ForegroundColor = LoggingConfig.ColorsSection.GetColorForLevel(level);

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