using NLoggify.Logging.Config;

namespace NLoggify.Logging.Loggers.Storage
{
    /// <summary>
    /// Base class for file-based loggers, handling file writing operations.
    /// Specific log formats (e.g., PlainText, JSON) should extend this class.
    /// </summary>
    internal class FileLogger : Logger
    {
        private readonly string _filePath;
        private readonly object _fileLock = new(); // Lock for thread-safe writing

        /// <summary>
        /// Initializes a new instance of the <see cref="FileLogger"/> class.
        /// </summary>
        /// <param name="filePath">The file path where logs will be written.</param>
        protected FileLogger(string filePath)
        {
            // Validate path
            ConfigValidation.ValidatePath(filePath);

            _filePath = filePath;

            // Ensure the directory exists
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        protected override void WriteLog(LogLevel level, string message, string timestamp)
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
