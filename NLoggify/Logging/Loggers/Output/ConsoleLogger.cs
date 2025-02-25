using NLoggify.Logging.Config;
using System.Diagnostics.CodeAnalysis;

namespace NLoggify.Logging.Loggers.Output
{
    /// <summary>
    /// A logger that writes log messages to the console (standard output).
    /// </summary>
    internal class ConsoleLogger : Logger
    {
        private static readonly object _lock = new object(); // Lock object for thread safety
        public override void Log(LogLevel level, string message)
        {
            lock (_lock)
            {
                // Change the console color based on the log level
                Console.ForegroundColor = LogLevelColorConfig.GetColorForLevel(level);
                base.Log(level, message);
                // Reset the console color back to the default
                Console.ResetColor();
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

        [ExcludeFromCodeCoverage] // No reason to test it
        public override void Dispose() { }
    }
}