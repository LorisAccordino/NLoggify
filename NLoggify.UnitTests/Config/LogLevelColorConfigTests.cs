using NLoggify.Logging.Config;
using NLoggify.Logging.Config.Enums;
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
            ConsoleColor actualColor = new LoggingConfig().ColorsSection.GetColorForLevel(unknownLogLevel);

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
            LoggingConfig config = new LoggingConfig();
            config.ColorsSection.ConfigureLogLevelColor(testLevel, newColor);
            ConsoleColor actualColor = config.ColorsSection.GetColorForLevel(testLevel);

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
            LoggingConfig.LogLevelColorConfig colorsConfig = new LoggingConfig().ColorsSection;
            colorsConfig.ConfigureLogLevelColors(customColors);

            // Assert
            Assert.Equal(ConsoleColor.DarkBlue, colorsConfig.GetColorForLevel(LogLevel.Info));
            Assert.Equal(ConsoleColor.Gray, colorsConfig.GetColorForLevel(LogLevel.Error));
        }
    }
}
