using NLoggify.Logging.Config;
using NLoggify.Logging.Config.Enums;
using NLoggify.Logging.Loggers;
using NLoggify.Logging.Loggers.Output;
using NLoggify.Logging.Loggers.Storage;
using NLoggify.Utils;
using System.Diagnostics.CodeAnalysis;

namespace NLoggify.UnitTests.Config
{
    /// <summary>
    /// Unit tests for verifying the logging configuration.
    /// </summary>
    [Collection("SequentialTests")]
    [ExcludeFromCodeCoverage]
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

            // Act
            LoggingConfig config = new LoggingConfig();
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
                ConfigValidation.ValidateTimestampFormat(timestampFormat);

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
                if (!ConfigValidation.ValidatePath(filePath, true, true)) throw new ArgumentException();

                // Assert
                if (shouldThrowException) Assert.Fail("Expected exception not thrown.");
            }
            catch (ArgumentException)
            {
                if (!shouldThrowException) Assert.Fail("Unexpected exception thrown.");
            }
        }

        /// <summary>
        /// Tests that ConfigureMultiLogger correctly adds multiple loggers.
        /// </summary>
        [Fact]
        public void ConfigureMultiLogger_AddsMultipleLoggers()
        {
            // Act
            LoggingConfig config = new LoggingConfig();
            var loggers = config._ConfigureMultiLogger(LoggerType.Console, LoggerType.PlainText);

            // Assert
            Assert.Equal(2, loggers.Count);
            Assert.IsType<ConsoleLogger>(loggers[0]);
            Assert.IsType<PlainTextLogger>(loggers[1]);
        }

        /// <summary>
        /// Tests that ConfigureMultiLogger does not add duplicate loggers.
        /// </summary>
        [Fact]
        public void ConfigureMultiLogger_DuplicateLogger_ThrowsException()
        {
            // Act & Assert
            LoggingConfig config = new LoggingConfig();
            var ex = Assert.Throws<ArgumentException>(() =>
                config.ConfigureMultiLogger(LoggerType.Console, LoggerType.Console)
            );

            Assert.Contains("has already been added.", ex.Message);
        }

        /// <summary>
        /// Tests that ConfigureMultiLogger prevents adding MultiLogger type.
        /// </summary>
        [Fact]
        public void ConfigureMultiLogger_MultiLogger_ThrowsException()
        {
            // Act & Assert
            LoggingConfig config = new LoggingConfig();
            var ex = Assert.Throws<ArgumentException>(() =>
                config.ConfigureMultiLogger(LoggerType.Multi)
            );

            Assert.Contains("A MultiLogger", ex.Message);
        }

        /// <summary>
        /// Tests that calling ConfigureMultiLogger without parameters results in an empty logger list.
        /// </summary>
        [Fact]
        public void ConfigureMultiLogger_NoLoggers_ResultsInEmptyList()
        {
            // Act
            LoggingConfig config = new LoggingConfig();
            var loggers = config._ConfigureMultiLogger();

            // Assert
            Assert.Empty(loggers);
        }

        /// <summary>
        /// Tests that ConfigureMultiLogger clears the previous loggers before adding new ones.
        /// </summary>
        [Fact]
        public void ConfigureMultiLogger_ClearsPreviousLoggers()
        {
            // Arrange
            LoggingConfig config = new LoggingConfig();
            var loggers = config._ConfigureMultiLogger(LoggerType.Console);
            Assert.Single(loggers); // Verify there's one logger

            // Act
            loggers = config._ConfigureMultiLogger(LoggerType.PlainText); // Reconfigure with a different logger

            // Assert
            Assert.Single(loggers); // Should still have only one logger
            Assert.IsType<PlainTextLogger>(loggers[0]); // The old ConsoleLogger should be replaced
        }

        /// <summary>
        /// Tests that GetLoggerBasedOnType correctly returns an instance of the expected logger.
        /// </summary>
        [Theory]
        [InlineData(LoggerType.Debug, typeof(DebugLogger))]
        [InlineData(LoggerType.Console, typeof(ConsoleLogger))]
        [InlineData(LoggerType.PlainText, typeof(PlainTextLogger))]
        [InlineData(LoggerType.JSON, typeof(JsonLogger))]
        [InlineData(LoggerType.Multi, typeof(MultiLogger))]
        public void GetLoggerBasedOnType_ReturnsCorrectLogger(LoggerType type, Type expectedType)
        {
            // Act
            var logger = LoggingConfig.GetLoggerBasedOnType(type);

            // Assert
            Assert.NotNull(logger);
            Assert.IsType(expectedType, logger);
        }

        /// <summary>
        /// Tests that GetLoggerBasedOnType throws NotSupportedException for an unsupported LoggerType.
        /// </summary>
        [Fact]
        public void GetLoggerBasedOnType_UnsupportedType_ThrowsNotSupportedException()
        {
            // Arrange
            var invalidType = (LoggerType)999; // A non valid value

            // Act & Assert
            Assert.Throws<NotSupportedException>(() => LoggingConfig.GetLoggerBasedOnType(invalidType));
        }
    }
}