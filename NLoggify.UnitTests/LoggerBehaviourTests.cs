using NLoggify.Logging.Loggers;
using NLoggify.Logging.Config;
using NLoggify.UnitTests.Utils;

namespace NLoggify.UnitTests
{
    /// <summary>
    /// Unit tests for verifying the behavior of the logger and its configuration.
    /// </summary>
    [Collection("SequentialTests")]
    [DebugOnly]
    public class LoggerBehaviourTests
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

        /// <summary>
        /// Tests if the logger respects the configured log level and logs the message accordingly.
        /// This test runs on multiple logger types.
        /// </summary>
        /// <param name="configLevel">The log level set in the configuration.</param>
        /// <param name="messageLevel">The log level of the message being logged.</param>
        /// <param name="shouldLog">A boolean indicating whether the message should be logged.</param>
        /// <param name="loggerType">The type of logger to test (Console, File, Memory, etc.).</param>
        [Theory]
        [InlineData(LogLevel.Info, LogLevel.Debug, false, LoggerType.Console)]  // Debug is lower than Info, so it doesn't log
        [InlineData(LogLevel.Warning, LogLevel.Error, true, LoggerType.Console)]  // Error is higher than Warning, so it logs
        [InlineData(LogLevel.Debug, LogLevel.Debug, true, LoggerType.Debug)]  // Debug = Debug, so it logs
        [InlineData(LogLevel.Error, LogLevel.Info, false, LoggerType.PlainText)]  // Info is lower than Error, so it doesn't log
        [InlineData(LogLevel.Warning, LogLevel.Warning, true, LoggerType.JSON)]  // Equal levels should log
        [InlineData(LogLevel.Critical, LogLevel.Error, false, LoggerType.PlainText)]  // Error < Critical, so it doesn't log
        [InlineData(LogLevel.Info, LogLevel.Critical, true, LoggerType.Console)]  // Critical is always logged
        public void Logger_ShouldLogProperly(LogLevel configLevel, LogLevel messageLevel, bool shouldLog, LoggerType loggerType)
        {
            // Arrange
            LoggingConfig.Configure(configLevel, loggerType);
            var logger = Logger.GetLogger();

            // Act
            logger.Log(messageLevel, "Test message");

            // Assert
            var output = Logger.GetDebugOutput(); // Simulated output for Console and Memory loggers
            if (shouldLog)
                Assert.Contains("Test message", output);
            else
                Assert.DoesNotContain("Test message", output);
        }


        [Fact]
        public async Task LogException_ShouldCatchAsyncExceptions()
        {
            // Arrange
            LoggingConfig.Configure(LogLevel.Info, LoggerType.Console);
            var logger = Logger.GetLogger();

            // Act
            bool exceptionCaught = await logger.LogException(LogLevel.Error, async () =>
            {
                await Task.Delay(50);
                throw new InvalidOperationException("Test error");
            });

            // Assert
            Assert.True(exceptionCaught);
        }
    }
}
