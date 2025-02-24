using NLoggify.Logging.Config;

namespace NLoggify.Logging.Loggers.Output
{
    /// <summary>
    /// A logger that writes log messages to the console (standard output).
    /// </summary>
    internal class ConsoleLogger : Logger
    {
        protected override void WriteLog(LogLevel level, string message, string timestamp)
        {
            // Change the console color based on the log level
            Console.ForegroundColor = LogLevelColorConfig.GetColorForLevel(level);

            // Print the log message with the formatted timestamp
            string logLine = $"[{timestamp}] {level}: {message}";
            Console.WriteLine(logLine);
            #if DEBUG
            debugOutputRedirect = logLine;
            #endif

            // Reset the console color back to the default
            Console.ResetColor();
        }

        public override void Dispose() { }
    }
}