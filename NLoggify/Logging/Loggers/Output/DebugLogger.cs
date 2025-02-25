using NLoggify.Logging.Config;
using NLoggify.Logging.Loggers;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace NLoggify.Logging.Loggers.Output
{
    /// <summary>
    /// A logger that writes log messages to the Debug output (e.g., Output window in Visual Studio).
    /// </summary>
#if !DEBUG
    [ExcludeFromCodeCoverage] // No reason to test it
#endif
    internal class DebugLogger : Logger
    {
        protected override void WriteLog(string header, string message)
        {
            // Print the log message with the formatted timestamp
            string logLine = header + message;
            Debug.WriteLine(logLine);
#if DEBUG
            debugOutputRedirect = logLine;
#endif
        }

        [ExcludeFromCodeCoverage] // No reason to test it
        public override void Dispose() { }
    }
}
