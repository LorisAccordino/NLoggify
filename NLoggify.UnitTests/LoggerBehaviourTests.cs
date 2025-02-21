using NLoggify.Logging.Loggers;
using NLoggify.Logging.Config;

namespace NLoggify.Tests
{
    /// <summary>
    /// Unit tests for verifying the behavior of the logger and its configuration.
    /// </summary>
    public class LoggerBehaviourTests
    {
        /// <summary>
        /// Tests if the logger respects the configured log level and logs the message accordingly.
        /// </summary>
        /// <param name="configLevel">The log level set in the configuration.</param>
        /// <param name="messageLevel">The log level of the message being logged.</param>
        /// <param name="shouldLog">A boolean indicating whether the message should be logged.</param>
        [Theory]
        [InlineData(LogLevel.Info, LogLevel.Debug, false)] // Debug is lower than Info, so it doesn't log
        [InlineData(LogLevel.Warning, LogLevel.Error, true)] // Error is higher than Warning, so it logs
        public void Logger_ShouldRespectLogLevel(LogLevel configLevel, LogLevel messageLevel, bool shouldLog)
        {
            // Arrange
            LoggingConfig.Configure(configLevel, LoggerType.Console);
            var logger = Logger.GetLogger();

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw); // Redirect the output of the console to catch the log

                // Act
                logger.Log(messageLevel, "Test message");

                // Assert
                var output = sw.ToString();
                if (shouldLog)
                    Assert.Contains("Test message", output);
                else
                    Assert.DoesNotContain("Test message", output);
            }
        }
    }
}
