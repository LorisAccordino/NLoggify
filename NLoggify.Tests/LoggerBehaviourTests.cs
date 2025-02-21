using Xunit;
using NLoggify.Logging;
using System.IO;
using NLoggify.Logging.Loggers;

namespace NLoggify.Tests
{
    public class LoggerBehaviourTests
    {
        [Theory]
        [InlineData(LogLevel.Info, LogLevel.Debug, false)] // Debug è inferiore a Info, quindi non logga
        [InlineData(LogLevel.Warning, LogLevel.Error, true)] // Error è superiore a Warning, quindi logga
        public void Logger_ShouldRespectLogLevel(LogLevel configLevel, LogLevel messageLevel, bool shouldLog)
        {
            // Arrange
            LoggingConfig.Configure(configLevel, LoggerType.Console);
            var logger = Logger.GetLogger();

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw); // Reindirizza l'output della console per intercettare il log

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
