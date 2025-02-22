using NLoggify.Logging.Config;

namespace NLoggify.Tests
{
    public class LoggingConfigTests
    {
        /// <summary>
        /// Tests if the logger is correctly configured with the specified settings.
        /// </summary>
        [Fact]
        public void Configure_ShouldSetCorrectValues()
        {
            // Arrange
            var expectedLevel = LogLevel.Warning;
            var expectedType = LoggerType.PlainText;
            var expectedPath = "output.log";

            // Act
            LoggingConfig.Configure(expectedLevel, expectedType, expectedPath);

            // Assert
            Assert.Equal(expectedLevel, LoggingConfig.MinimumLogLevel);
            Assert.Equal(expectedType, LoggingConfig.LoggerType);
            Assert.Equal(expectedPath, LoggingConfig.FilePath);
        }

        /// <summary>
        /// Tests if the timestamp format is validated correctly during the logger configuration.
        /// </summary>
        /// <param name="timestampFormat">The timestamp format string to be validated.</param>
        /// <param name="shouldThrowException">Indicates whether an exception should be thrown for the given format.</param>
        [Theory]
        [InlineData("", false)] // Empty format is invalid, so it should throw
        [InlineData("WrongFormat", true)] // Invalid format, should throw an exception
        [InlineData("d/M/yyyy H:m", false)] // Valid format, no exception should be thrown
        [InlineData("yyyy-MM-dd HH:mm:ss", false)] // Valid format, no exception should be thrown
        public void Configure_ShouldValidateTimestamp(string timestampFormat, bool shouldThrowException)
        {
            // Act
            try
            {
                LoggingConfig.Configure(LogLevel.Info, LoggerType.Console, "", timestampFormat);

                // Assert
                if (shouldThrowException)
                {
                    Assert.Fail("Expected exception not thrown.");
                }
            }
            catch (ArgumentException)
            {
                if (!shouldThrowException)
                {
                    Assert.Fail("Unexpected exception thrown.");
                }
            }
        }

        /// <summary>
        /// Tests if the log file path is validated correctly during logger configuration.
        /// </summary>
        /// <param name="filePath">The log file path to be validated.</param>
        /// <param name="shouldThrowException">Indicates whether an exception should be thrown for the given path.</param>
        [Theory]
        [InlineData("", false)] // Empty path is valid
        [InlineData("   ", true)] // Path with only spaces is invalid
        [InlineData("C:\\Valid\\Path\\log.txt", false)] // Valid path
        [InlineData("C:/Another/Valid/Path/log.log", false)] // Valid Unix-style path
        [InlineData("C:\\Path\\With\\Illegal|Char.txt", true)] // Invalid path due to illegal characters
        [InlineData("/invalid_path_with_*?.txt", true)] // Invalid path due to wildcard characters
        public void Configure_ShouldValidateLogFilePath(string filePath, bool shouldThrowException)
        {
            // Act
            try
            {
                LoggingConfig.Configure(LogLevel.Info, LoggerType.PlainText, filePath);

                // Assert
                if (shouldThrowException)
                {
                    Assert.Fail("Expected exception not thrown.");
                }
            }
            catch (ArgumentException)
            {
                if (!shouldThrowException)
                {
                    Assert.Fail("Unexpected exception thrown.");
                }
            }
        }
    }
}