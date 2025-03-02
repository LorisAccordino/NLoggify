using NLoggify.Logging.Config;
using NLoggify.Logging.Config.Enums;
using System.Diagnostics.CodeAnalysis;

namespace NLoggify.UnitTests.Config
{
    /// <summary>
    /// Unit tests for verifying the log level color and other console configuration properties.
    /// </summary>
    [Collection("SequentialTests")]
    [ExcludeFromCodeCoverage]
    public class ConsoleLoggerConfigTests
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
            ConsoleColor actualColor = new ConsoleLoggerConfig().GetColorForLevel(unknownLogLevel);

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
            ConsoleLoggerConfig config = new ConsoleLoggerConfig();
            config.ConfigureLogLevelColor(testLevel, newColor);
            ConsoleColor actualColor = config.GetColorForLevel(testLevel);

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
            ConsoleLoggerConfig config = new ConsoleLoggerConfig();
            config.ConfigureLogLevelColors(customColors);

            // Assert
            Assert.Equal(ConsoleColor.DarkBlue, config.GetColorForLevel(LogLevel.Info));
            Assert.Equal(ConsoleColor.Gray, config.GetColorForLevel(LogLevel.Error));
        }
    }
}