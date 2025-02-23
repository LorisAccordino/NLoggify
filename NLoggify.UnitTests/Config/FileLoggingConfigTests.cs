using NLoggify.Logging.Config;

namespace NLoggify.UnitTests.Config
{
    /// <summary>
    /// Provides tests for verifying the behavior of the <see cref="FileLoggingConfig"/> class.
    /// Contains tests for file path validation, custom directory handling, timestamping of log files,
    /// and ensuring the existence of necessary directories.
    /// </summary>
    [Collection("SequentialTests")]
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

        /// <summary>
        /// Verifies that enabling timestamped log files generates a file name with a timestamp.
        /// The generated file name should include the timestamp and should differ from the default "output.log" file name.
        /// </summary>
        [Fact]
        public void EnableTimestampedLogFile_ShouldSetFileNameWithTimestamp()
        {
            var originalFilePath = FileLoggingConfig.FilePath;
            FileLoggingConfig.EnableTimestampedLogFile();
            var newFilePath = FileLoggingConfig.FilePath;

            Assert.DoesNotContain("output.log", newFilePath); // The filename should be changed
            Assert.StartsWith("log_", Path.GetFileName(newFilePath)); // The filename should start with with "log_"
            Assert.EndsWith(".log", newFilePath); // The filename should end with ".log"
            Assert.NotEqual(originalFilePath, newFilePath); // Check the changed has been done
        }

        /// <summary>
        /// Verifies that invalid characters in file names are replaced correctly.
        /// Specifically, characters like '|', '<', '>', and '*' are replaced with an underscore.
        /// </summary>
        [Fact]
        public void MakeValidFilename_ShouldReplaceInvalidChars()
        {
            var invalidFilename = "log|invalid<>filename*.log";
            var validFilename = FileLoggingConfig.MakeValidFilename(invalidFilename);
            Assert.Equal("log_invalid__filename_.log", validFilename);
        }

        /// <summary>
        /// Verifies that the log directory is created if it does not already exist.
        /// This ensures that logging can proceed without issues even if the directory needs to be created at runtime.
        /// </summary>
        [Fact]
        public void EnsureLogDirectoryExists_ShouldCreateDirectoryIfNotExists()
        {
            var directoryPath = @"C:\Custom\EnsureDirectory";
            FileLoggingConfig.SetCustomFilePath(Path.Combine(directoryPath, "output.log"));

            // Make sure the directory doesn't exist
            if (Directory.Exists(directoryPath)) Directory.Delete(directoryPath, true);

            FileLoggingConfig.EnsureLogDirectoryExists();

            // Check the directory has been created
            Assert.True(Directory.Exists(directoryPath));
        }
    }
}
