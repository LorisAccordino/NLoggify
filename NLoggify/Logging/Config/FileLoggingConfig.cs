using NLoggify.Utils;

namespace NLoggify.Logging.Config
{
    /// <summary>
    /// Configuration class for file-based logging.
    /// Provides methods to set and validate log file paths and directories.
    /// </summary>
    public class FileLoggingConfig
    {
        private static readonly object _lock = new();

        /// <summary>
        /// Gets the full file path used for file-based logging.
        /// </summary>
        public static string FilePath { get; private set; } = Path.Combine(Directory.GetCurrentDirectory(), "logs", "output.log");

        /// <summary>
        /// Sets a new <b>custom</b> file path for logging after validating its format.
        /// </summary>
        /// <param name="filePath">The new log file path.</param>
        /// <exception cref="ArgumentException">Thrown if the path is empty or contains invalid characters.</exception>
        public static void SetCustomFilePath(string filePath)
        {
            lock (_lock)
            {
                ConfigValidation.ValidatePath(filePath);
                FilePath = Path.GetFullPath(filePath);
            }
        }

        /// <summary>
        /// Sets the directory where log files will be stored, using a <b>default</b> filename.
        /// If the directory does not exist, it is automatically created.
        /// </summary>
        /// <param name="directoryPath">The target log directory.</param>
        /// <exception cref="ArgumentException">Thrown if the directory path is invalid.</exception>
        public static void SetLogDirectory(string directoryPath)
        {
            if (string.IsNullOrWhiteSpace(directoryPath))
                throw new ArgumentException("Directory path cannot be empty.");

            if (directoryPath.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
                throw new ArgumentException("Directory path contains invalid characters.");

            // Ensure the directory exists
            Directory.CreateDirectory(directoryPath);

            lock (_lock)
            {
                FilePath = Path.Combine(directoryPath, "output.log");
            }
        }

        /// <summary>
        /// Enables timestamped log files with the format "log_yyyy-MM-dd_HH-mm-ss.log".
        /// This allows logs to be stored with unique filenames based on creation time.
        /// </summary>
        public static void EnableTimestampedLogFile()
        {
            lock (_lock)
            {
                string directory = Path.GetDirectoryName(FilePath) ?? Directory.GetCurrentDirectory();
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                FilePath = Path.Combine(directory, $"log_{timestamp}.log");
            }
        }

        /// <summary>
        /// Ensures that the directory containing the log file exists.
        /// If the directory does not exist, it is automatically created.
        /// </summary>
        public static void EnsureLogDirectoryExists()
        {
            string directory = Path.GetDirectoryName(FilePath) ?? Directory.GetCurrentDirectory();
            Directory.CreateDirectory(directory);
        }
    }
}
