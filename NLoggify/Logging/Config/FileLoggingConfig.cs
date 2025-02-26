using NLoggify.Utils;

namespace NLoggify.Logging.Config
{

    public partial class LoggingConfig
    {
        /// <summary>
        /// Section about the file logging configuration
        /// </summary>
        public FileLoggingConfig FileSection { get; }

        /// <summary>
        /// Configuration class for file-based logging.
        /// Provides methods to set and validate log file paths and directories.
        /// </summary>
        public class FileLoggingConfig
        {
            private LoggingConfig loggingConfig; // Reference to CurrentConfig

            /// <summary>
            /// Gets the directory path used for file-based logging.
            /// </summary>
            public string DirectoryPath
            {
                get => _directoryPath;
                set { if (ConfigValidation.ValidatePath(value)) _directoryPath = value; }
            }
            private string _directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "logs");

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
                    string timestamp = DateTime.Now.ToString(loggingConfig.TimestampFormat); // Get the current DateTime formatted
                    timestamp = GenericUtils.MakeValidFilename(timestamp); // Validate timestamp
                    return $"{FileNamePrefix}_{timestamp}.log"; // Return the full file name
                }
            }

            /// <summary>
            /// Gets the full path of the log file
            /// </summary>
            public string FullPath { get => Path.Combine(DirectoryPath, LogFileName); }

            // Get reference to CurrentConfig
            internal FileLoggingConfig(LoggingConfig loggingConfig)
            {
                this.loggingConfig = loggingConfig;
            }
        }
    }
}
