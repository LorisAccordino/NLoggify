namespace NLoggify.Logging
{
    /// <summary>
    /// Enum representing different log levels for categorizing log messages.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Detailed trace messages, typically used for development and debugging.
        /// </summary>
        Trace,

        /// <summary>
        /// Debug messages providing additional details for debugging.
        /// </summary>
        Debug,

        /// <summary>
        /// Informational messages that track the normal flow of the application.
        /// </summary>
        Info,

        /// <summary>
        /// Warnings about potential issues that do not interrupt the program flow.
        /// </summary>
        Warning,

        /// <summary>
        /// Error messages indicating a failure in an operation.
        /// </summary>
        Error,

        /// <summary>
        /// Critical issues that may require immediate action, leading to potential program failure.
        /// </summary>
        Critical,

        /// <summary>
        /// Fatal errors that cause the program to terminate.
        /// </summary>
        Fatal
    }
}
