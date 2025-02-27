using NLoggify.Logging.Config;
using NLoggify.Logging.Config.Enums;
using NLoggify.Logging.Loggers;
using NLoggify.Logging.Loggers.Output;
using NLoggify.Logging.Loggers.Storage;
using System.Diagnostics.CodeAnalysis;

namespace NLoggify.UnitTests.Config
{
    /// <summary>
    /// Unit tests for verifying the logging configuration.
    /// </summary>
    [Collection("SequentialTests")]
    [ExcludeFromCodeCoverage]
    public class LoggerConfigTests
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

            // Act
            LoggerConfig config = new LoggerConfig();
            config.MinimumLogLevel = expectedLevel;
            config.LoggerType = expectedType;

            // Assert
            Assert.Equal(expectedLevel, config.MinimumLogLevel);
            Assert.Equal(expectedType, config.LoggerType);
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
                ConfigValidator.ValidateTimestampFormat(timestampFormat);

                // Assert
                if (shouldThrowException) Assert.Fail("Expected exception not thrown.");
            }
            catch (ArgumentException)
            {
                if (!shouldThrowException) Assert.Fail("Unexpected exception thrown.");
            }
        }

        /// <summary>
        /// Tests if the log file path is validated correctly during logger configuration.
        /// </summary>
        /// <param name="filePath">The log file path to be validated.</param>
        /// <param name="shouldThrowException">Indicates whether an exception should be thrown for the given path.</param>
        /// <param name="hasFilename">Indicates whether to test even the filename or not.</param>
        [Theory]
        [InlineData(null, true)] // Path is null
        [InlineData("", true)] // Empty path is invalid
        [InlineData("   ", true)] // Path with only spaces is invalid
        [InlineData("C:\\Valid\\Path\\log.txt", false)] // Valid path
        [InlineData("C:\\Valid\\Path", false)]  // Valid path without filename required
        [InlineData("C:/Another/Valid/Path/log.log", false)] // Valid Unix-style path
        [InlineData("C:\\Path\\With\\Illegal|Char.txt", true)] // Invalid path due to illegal characters
        [InlineData("/invalid_path_with_*?.txt", true)] // Invalid path due to wildcard characters
        [InlineData("C:\\Folder\\", true)] // Empty filename
        [InlineData("C:\\", true)] // Empty folder (only root)
        [InlineData(@"C:/Folder\InvalidPath", false)] // Mixed path
        //[InlineData(new string('a', 270), false)] // Too long name
        public void Configure_ShouldValidateLogFilePath(string filePath, bool shouldThrowException)
        {
            // Act
            try
            {
                if (!ConfigValidator.ValidatePath(filePath, true, true)) throw new ArgumentException();

                // Assert
                if (shouldThrowException) Assert.Fail("Expected exception not thrown.");
            }
            catch (ArgumentException)
            {
                if (!shouldThrowException) Assert.Fail("Unexpected exception thrown.");
            }
        }
    }
}