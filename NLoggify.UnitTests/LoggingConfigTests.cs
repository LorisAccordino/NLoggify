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
        /// Tests if an exception is (as expected) thrown if the given path doesn't exist
        /// </summary>
        [Fact]
        public void Configure_ShouldThrowException_IfLogFilePathIsInvalid()
        {
            // Arrange
            string invalidFilePath = "Z:\\cartella\\inesistente\\log.txt";

            // Act & Assert
            var ex = Assert.Throws<IOException>(() => LoggingConfig.Configure(LogLevel.Info, LoggerType.PlainText, invalidFilePath));
            Assert.Contains("Cannot access to the given path", ex.Message);
        }

    }
}