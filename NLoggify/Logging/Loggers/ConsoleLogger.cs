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
            switch (level)
            {
                case LogLevel.Trace:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case LogLevel.Debug:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case LogLevel.Info:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case LogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case LogLevel.Critical:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogLevel.Fatal:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }

            // Print the log message with the formatted timestamp
            Console.WriteLine($"[{formattedTimestamp}] {level}: {message}");

            // Reset the console color back to the default
            Console.ResetColor();
        }


        public override void Dispose()
        {
            // Implement the logic to release resources, e.g.:
            // - Close any output streams
            // - Release unmanaged resources
            throw new NotImplementedException("The Dispose method is not implemented yet.");
        }
    }
}
