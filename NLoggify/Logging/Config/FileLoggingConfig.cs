using System.Diagnostics.CodeAnalysis;

namespace NLoggify.Logging.Config
{
    /// <summary>
    /// Configuration class for file-based logging.
    /// Provides methods to set and validate log file paths and directories.
    /// </summary>
    public static class FileLoggingConfig
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
                ConfigValidation.ValidatePath(filePath, true, true);
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
            ConfigValidation.ValidatePath(directoryPath, false);

            // Ensure the directory exists
            Directory.CreateDirectory(directoryPath);

            lock (_lock)
            {
                FilePath = Path.Combine(directoryPath, "output.log");
            }
        }

        /// <summary>
        /// Enables timestamped log files with the format "log_yyyy-MM-dd_HH-mm-ss.log".
        /// Ensures that the timestamp format is compatible with file naming.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the timestamp format cannot be used in a file name.</exception>
        [ExcludeFromCodeCoverage] // No reason to test it
        internal static void EnableTimestampedLogFile()
        {
            lock (_lock)
            {
                string directory = Path.GetDirectoryName(FilePath) ?? Directory.GetCurrentDirectory();
                string timestamp = DateTime.Now.ToString(LoggingConfig.TimestampFormat);

                // Ensure the timestamp is valid for filenames
                string validTimestamp = MakeValidFilename(timestamp);

                if (string.IsNullOrWhiteSpace(validTimestamp))
                {
                    throw new ArgumentException("The generated timestamp is invalid for a file name. Please check the format.");
                }

                FilePath = Path.Combine(directory, $"log_{validTimestamp}.log");
            }
        }

        /// <summary>
        /// Ensures that the given string is a valid filename by replacing invalid characters.
        /// </summary>
        /// <param name="filename">The filename to validate.</param>
        /// <returns>A valid filename or an empty string if the correction is impossible.</returns>
        [ExcludeFromCodeCoverage] // No reason to test it
        internal static string MakeValidFilename(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename)) return string.Empty;

            char[] invalidChars = Path.GetInvalidFileNameChars();
            string validFilename = new string(filename.Select(c => invalidChars.Contains(c) ? '_' : c).ToArray());

            return string.IsNullOrWhiteSpace(validFilename) ? string.Empty : validFilename;
        }

        /// <summary>
        /// Ensures that the directory containing the log file exists.
        /// If the directory does not exist, it is automatically created.
        /// </summary>
        [ExcludeFromCodeCoverage] // No reason to test it
        internal static void EnsureLogDirectoryExists()
        {
            string directory = Path.GetDirectoryName(FilePath) ?? Directory.GetCurrentDirectory();
            Directory.CreateDirectory(directory);
        }
    }
}
