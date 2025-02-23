﻿using NLoggify.Logging.Config;
using System.Diagnostics.CodeAnalysis;

namespace NLoggify.UnitTests.Config
{
    /// <summary>
    /// Provides tests for verifying the behavior of the <see cref="FileLoggingConfig"/> class.
    /// Contains tests for file path validation, custom directory handling, timestamping of log files,
    /// and ensuring the existence of necessary directories.
    /// </summary>
    [Collection("SequentialTests")]
    [ExcludeFromCodeCoverage]
    public class FileLoggingConfigTests
    {
        /// <summary>
        /// Tests setting a custom file path and verifies that the new path is correctly set.
        /// Valid paths are tested. Invalid paths result in an exception being thrown.
        /// </summary>
        /// <param name="filePath">The file path to set for the log file.</param>
        /// <param name="shouldSucceed">Indicates whether the test should succeed (true for valid paths, false for invalid ones).</param>
        [Theory]
        [InlineData(@"C:\Custom\path\logfile.log", true)]
        [InlineData("invalid|path/logfile.log", false)]
        [InlineData("", false)]
        public void SetCustomFilePath_ShouldHandleVariousPaths(string filePath, bool shouldSucceed)
        {
            if (shouldSucceed)
            {
                FileLoggingConfig.SetCustomFilePath(filePath);
                Assert.Equal(Path.GetFullPath(filePath), FileLoggingConfig.FilePath);
            }
            else
            {
                Assert.Throws<ArgumentException>(() => FileLoggingConfig.SetCustomFilePath(filePath));
            }
        }

        /// <summary>
        /// Tests setting a custom log directory and verifies that the directory is created.
        /// Also checks if the file path is updated correctly when a valid directory is set.
        /// Invalid directory paths should throw an exception.
        /// </summary>
        /// <param name="directoryPath">The directory to set for log files.</param>
        /// <param name="shouldSucceed">Indicates whether the test should succeed (true for valid paths, false for invalid ones).</param>
        [Theory]
        [InlineData(@"C:\Custom\Logs", true)]
        [InlineData("invalid|directory", false)]
        public void SetLogDirectory_ShouldHandleVariousDirectories(string directoryPath, bool shouldSucceed)
        {
            if (shouldSucceed)
            {
                FileLoggingConfig.SetLogDirectory(directoryPath);
                var expectedFilePath = Path.Combine(directoryPath, "output.log");
                Assert.Equal(expectedFilePath, FileLoggingConfig.FilePath);
                Assert.True(Directory.Exists(directoryPath)); // Verify the directory has been created
                Directory.Delete(directoryPath);
            }
            else
            {
                Assert.Throws<ArgumentException>(() => FileLoggingConfig.SetLogDirectory(directoryPath));
            }
        }

        /*
        /// <summary>
        /// Tests that the log directory is correctly set and the file path is generated properly
        /// when enabling a timestamped log file.
        /// </summary>
        /// <param name="filePath">The custom file path provided, or null to use the default.</param>
        /// <param name="timestampFormat">The format used for the timestamp in the log file name.</param>
        [Theory]
        [InlineData(null, "yyyy-MM-dd_HH-mm-ss")] // Uses default path with a valid timestamp format
        [InlineData(null, "")] // Uses default path but with an empty timestamp format
        public void EnsureLogDirectoryExists_ValidPath_FilePathIsSetCorrectly(string filePath, string timestampFormat)
        {
            // Arrange
            string expectedDirectory = Directory.GetCurrentDirectory(); // Default log directory

            // Act & Assert
            if (string.IsNullOrWhiteSpace(timestampFormat))
            {
                // If the timestamp format is empty, an exception is expected
                Assert.Throws<ArgumentException>(() => FileLoggingConfig.EnableTimestampedLogFile(filePath, timestampFormat));
            }
            else
            {
                // If the format is valid, the method should work propertly
                FileLoggingConfig.EnableTimestampedLogFile(filePath, timestampFormat);

                // Assert
                Assert.StartsWith(Path.Combine(expectedDirectory, "log_"), FileLoggingConfig.FilePath); // Check if log file is in the correct directory
                Assert.EndsWith(".log", FileLoggingConfig.FilePath); // Ensure the log file has the correct extension
            }
        }
        */
    }
}
