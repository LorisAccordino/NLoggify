using NLoggify.Logging.Config;
using System.Diagnostics;

namespace NLoggify.Logging.Loggers.Output
{
    /// <summary>
    /// A logger that writes log messages to the Debug output (e.g., Output window in Visual Studio).
    /// </summary>
    internal class DebugLogger : Logger
    {
        protected override void WriteLog(LogLevel level, string message, string timestamp)
        {
            // Print the log message with the formatted timestamp
            string logLine = $"[{timestamp}] {level}: {message}";
            Debug.WriteLine(logLine);
            #if DEBUG
            debugOutputRedirect = logLine;
            #endif
        }

        public override void Dispose() { }
    }
}
