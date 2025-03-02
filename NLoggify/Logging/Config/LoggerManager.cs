using NLoggify.Logging.Loggers;

namespace NLoggify.Logging.Config
{
    /// <summary>
    /// A centralized manager that serves as a clean entry point of the entire logging system
    /// </summary>
    public static class LoggerManager
    {
        private static ILogger? loggerInstance;
        private static LoggerBuilder builder = new LoggerBuilder();

        /// <summary> Get the <see cref="LoggerBuilder"/> reference to configure the logging system </summary>
        public static LoggerBuilder Configure() => builder;

        /// <summary> Gets the (singleton) reference of the logger. This has to be used in the entire logging system. </summary>
        public static ILogger GetLogger() => loggerInstance ?? builder.Default().Build();

        internal static void SetLogger(ILogger logger) => loggerInstance = logger;
    }

}
