namespace NLoggify.Logging.Config
{
    /// <summary>
    /// Specifies the available types of loggers that can be used in the logging system.
    /// </summary>
    public enum LoggerType
    {
        /// <summary>
        /// Logs messages to the system console.
        /// </summary>
        Console,

        /// <summary>
        /// Logs messages to a plain text file.
        /// </summary>
        PlainText,

        /// <summary>
        /// Logs messages in a structured JSON format.
        /// </summary>
        JSON
    }
}