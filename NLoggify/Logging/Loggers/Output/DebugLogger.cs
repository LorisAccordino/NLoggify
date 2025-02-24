using NLoggify.Logging.Config;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace NLoggify.Logging.Loggers.Output
{
    /// <summary>
    /// A logger that writes log messages to the Debug output (e.g., Output window in Visual Studio).
    /// </summary>
    #if DEBUG
    public class DebugLogger : Logger
    #else
    internal class DebugLogger : Logger
    #endif
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

        [ExcludeFromCodeCoverage] // No reason to test it
        public override void Dispose() { }
    }
}
