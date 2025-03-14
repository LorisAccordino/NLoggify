﻿using NLoggify.Logging.Config;
using System.Diagnostics;

namespace NLoggify.Logging.Loggers.Transient
{
    /// <summary>
    /// A logger that writes log messages to the Debug output (e.g., Output window in Visual Studio).
    /// </summary>
#if !DEBUG
    [ExcludeFromCodeCoverage] // No reason to test it
#endif
    internal class DebugLogger : Logger
    {
        internal DebugLogger() { }
        internal DebugLogger(LoggerConfig config) : base(config) { }

        protected override void WriteLog(string header, string message)
        {
            // Print the log message with the formatted timestamp
            string logLine = header + message;
            Debug.WriteLine(logLine);
#if DEBUG
            debugOutputRedirect = logLine;
#endif
        }
    }
}
