using NLoggify.Logging.Loggers;
using NLoggify.Logging.Config;

namespace NLoggify.Tests
{
    /// <summary>
    /// Unit tests for the <see cref="Logger"/> singleton behavior.
    /// </summary>
    public class LoggerTests
    {
        /// <summary>
        /// Ensures that <see cref="Logger.GetLogger"/> always returns a not null value,
        /// both before and after configuration.
        /// </summary>
        /// <param name="configure">Whether to configure the logger before testing.</param>
        [Theory]
        [InlineData(false)] // Test before configuration
        [InlineData(true)]  // Test after configuration
        public void LoggerInstance_ShouldNotBeNull(bool configure)
        {
            // Arrange
            if (configure)
            {
                LoggingConfig.Configure(LogLevel.Info, LoggerType.Console);
            }

            // Act
            var logger = Logger.GetLogger();

            // Assert
            Assert.NotNull(logger);
        }

        /// <summary>
        /// Ensures that <see cref="Logger.GetLogger"/> always returns the same instance.
        /// </summary>
        [Fact]
        public void LoggerInstance_ShouldReturnSameInstance()
        {
            // Arrange
            LoggingConfig.Configure(LogLevel.Info, LoggerType.Console);

            // Act
            var logger1 = Logger.GetLogger();
            var logger2 = Logger.GetLogger();

            // Assert
            Assert.Same(logger1, logger2); // Must be the same instance
        }
    }
}