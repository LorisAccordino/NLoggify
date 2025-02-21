using NLoggify.Logging;

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

            // Act
            LoggingConfig.Configure(expectedLevel, expectedType);

            // Assert
            Assert.Equal(expectedLevel, LoggingConfig.LogLevel);
            Assert.Equal(expectedType, LoggingConfig.LoggerType);
        }
    }
}