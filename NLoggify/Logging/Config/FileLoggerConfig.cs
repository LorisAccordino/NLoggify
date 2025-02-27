using NLoggify.Utils;

namespace NLoggify.Logging.Config
{

    /// <summary>
    /// Configuration class for file-based logging.
    /// Provides methods to set and validate log file paths and directories.
    /// </summary>
    public class FileLoggerConfig : LoggerConfig
    {
        /// <summary>
        /// Gets the directory path used for file-based logging.
        /// </summary>
        public string DirectoryPath
        {
            get => directoryPath;
            set { if (ConfigValidator.ValidatePath(value)) directoryPath = value; }
        }
        private string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "logs");

        /// <summary>
        /// Gets or sets the prefix used for the log file name.
        /// If not set, the default value is "log". <br></br>
        /// <b>Note:</b> The full name will be the prefix and the current timestamp.
        /// Example: <i>log_2025-02-25 16_05_38.log</i>
        /// </summary>
        public string FileNamePrefix { get; set; } = "log";

        /// <summary>
        /// Gets the full log file name including the prefix and the current (<see cref="DateTime.Now"/>) timestamp. <br></br>
        /// Example: <i>log_2025-02-25 16_05_38.log</i> or <i>output_2025-02-25 16_05_38.log</i>
        /// </summary>
        public string LogFileName
        {
            get
            {
                string timestamp = DateTime.Now.ToString(TimestampFormat); // Get the current DateTime formatted
                timestamp = GenericUtils.MakeValidFilename(timestamp); // Validate timestamp
                return $"{FileNamePrefix}_{timestamp}.log"; // Return the full file name
            }
        }

        /// <summary>
        /// Gets the full path of the log file
        /// </summary>
        public string FullPath { get => Path.Combine(DirectoryPath, LogFileName); }
    }
}
