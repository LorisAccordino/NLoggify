using NLoggify.Logging.Loggers;

namespace NLoggify.Logging.Config
{
    /// <summary>
    /// A centralized manager that serves as a clean entry point of the entire logging system
    /// </summary>
    public static class LoggerManager
    {
        private static ILogger? loggerInstance;

        /// <summary>
        /// Gets the singleton instance of the logger. This has to be used in the entire logging system.
        /// </summary>
        /// <returns>Logger instance of the entire logging system.</returns>
        public static ILogger GetLogger()
        {
            if (loggerInstance == null)
            {
                ShowWarning();
                loggerInstance = LoggerBuilder.Default().Build();
            }

            return loggerInstance;
        }

        internal static void SetLogger(ILogger logger)
        {
            loggerInstance = logger;
        }

        private static void ShowWarning()
        {
            if (!LoggerBuilder.SuppressWarnings)
            {
                Console.WriteLine("⚠️ Warning: Logger not configured. Using default ConsoleLogger.");
                Console.WriteLine("ℹ️ You can suppress warnings by setting LoggerBuilder.SuppressWarnings = true");
            }
        }
    }

}
