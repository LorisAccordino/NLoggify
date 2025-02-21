using NLoggify.Logging.Config;

namespace NLoggify.Logging.Loggers
{
    /// <summary>
    /// A logger that writes log messages to the console (standard output).
    /// </summary>
    internal class ConsoleLogger : Logger
    {
        protected override void WriteLog(LogLevel level, string message, DateTime timestamp)
        {
            // Format the timestamp using the specified format
            string formattedTimestamp = timestamp.ToString(LoggingConfig.TimestampFormat);

            // Change the console color based on the log level
            Console.ForegroundColor = LogLevelColorConfig.GetColorForLevel(level);

            // Print the log message with the formatted timestamp
            Console.WriteLine($"[{formattedTimestamp}] {level}: {message}");

            // Reset the console color back to the default
            Console.ResetColor();
        }


        public override void Dispose() { }
    }
}
