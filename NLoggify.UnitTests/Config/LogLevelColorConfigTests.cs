using NLoggify.Logging.Config;
using NLoggify.UnitTests.Utils;
using System.Diagnostics.CodeAnalysis;

namespace NLoggify.UnitTests.Config
{
    /// <summary>
    /// Unit tests for verifying the log level color configuration.
    /// </summary>
    [Collection("SequentialTests")]
    [ExcludeFromCodeCoverage]
    public class LogLevelColorConfigTests
    {
        /// <summary>
        /// Verifies that an unknown log level returns the default ConsoleColor.White.
        /// </summary>
        [Fact]
        public void GetColorForLevel_UnknownLogLevel_ReturnsWhite()
        {
            // Arrange
            var unknownLogLevel = (LogLevel)999; // Unexisting log level

            // Act
            ConsoleColor actualColor = LogLevelColorConfig.GetColorForLevel(unknownLogLevel);

            // Assert
            Assert.Equal(ConsoleColor.White, actualColor);
        }

        /// <summary>
        /// Verifies that configuring a single log level updates the color correctly.
        /// </summary>
        [Fact]
        public void ConfigureLogLevelColor_UpdatesColorCorrectly()
        {
            // Arrange
            LogLevel testLevel = LogLevel.Warning;
            ConsoleColor newColor = ConsoleColor.Magenta;

            // Act
            LogLevelColorConfig.ConfigureLogLevelColor(testLevel, newColor);
            ConsoleColor actualColor = LogLevelColorConfig.GetColorForLevel(testLevel);

            // Assert
            Assert.Equal(newColor, actualColor);
        }

        /// <summary>
        /// Verifies that configuring multiple log levels updates all colors correctly.
        /// </summary>
        [Fact]
        public void ConfigureLogLevelColors_UpdatesMultipleColors()
        {
            // Arrange
            var customColors = new Dictionary<LogLevel, ConsoleColor>
        {
            { LogLevel.Info, ConsoleColor.DarkBlue },
            { LogLevel.Error, ConsoleColor.Gray }
        };

            // Act
            LogLevelColorConfig.ConfigureLogLevelColors(customColors);

            // Assert
            Assert.Equal(ConsoleColor.DarkBlue, LogLevelColorConfig.GetColorForLevel(LogLevel.Info));
            Assert.Equal(ConsoleColor.Gray, LogLevelColorConfig.GetColorForLevel(LogLevel.Error));
        }
    }
}
