using NLoggify.Logging.Loggers;
using NLoggify.Logging;

namespace NLoggify.Tests
{
    /// <summary>
    /// Unit tests for the <see cref="Logger"/> singleton behavior.
    /// </summary>
    public class LoggerTests
    {
        /// <summary>
        /// Ensures that <see cref="Logger.GetLogger"/> always returns a not null value after configuration.
        /// </summary>
        [Fact]
        public void LoggerInstance_ShouldNotBeNull_AfterConfiguration()
        {
            // Arrange
            LoggingConfig.Configure(LogLevel.Info, LoggerType.Console);

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