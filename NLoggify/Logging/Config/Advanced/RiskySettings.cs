namespace NLoggify.Logging.Config.Advanced
{
    /// <summary>
    /// Provides the possibility of "unlocking" risky advanced settings for advanced users. <br></br>
    /// It is not reccommended to enable these settings, unless you know what you do. Do it at your own risk!
    /// </summary>
    public static class RiskySettings
    {
        /// <summary>
        /// Allows the possibility of having more loggers with the same output destination (file, console, etc).
        /// </summary>
        public static bool AllowMultipleSameLoggers { get; set; } = false;

        /// <summary>
        /// Allows the hot-reload reconfiguration of the logger after the initial configuration
        /// </summary>
        public static bool AllowReconfiguration { get; set; } = false;

        /// <summary>
        /// Allows to set a default logger fallback behavior, without worrying about thrown exceptions
        /// </summary>
        public static bool AllowDefaultLogger { get; set; } = true;

        /// <summary>
        /// Allows async logging mode for advanced purposes
        /// </summary>
        //public static bool AlloAsyncLogging { get; set; } = false;
    }

}
