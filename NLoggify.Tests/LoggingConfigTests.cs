using NLoggify.Logging.Config;

namespace NLoggify.Tests
{
    public class LoggingConfigTests
    {
        [Fact]
        public void ConfigureLogging_ShouldSetCorrectValues()
        {
            // Arrange
            var expectedLevel = LogLevel.Warning;
            var expectedType = LoggerType.PlainText;
            var expectedPath = "output.log";
            var expectedTimestamp = "aaa";

            // Act
            LoggingConfig.Configure(expectedLevel, expectedType, expectedPath, expectedTimestamp);

            // Assert
            Assert.Equal(expectedLevel, LoggingConfig.MinimumLogLevel);
            Assert.Equal(expectedType, LoggingConfig.LoggerType);
            Assert.Equal(expectedPath, LoggingConfig.FilePath);
            Assert.Equal(expectedTimestamp, LoggingConfig.FilePath);
        }
    }
}