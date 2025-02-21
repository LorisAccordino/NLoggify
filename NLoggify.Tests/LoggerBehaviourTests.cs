using Xunit;
using System.IO;
using NLoggify.Logging.Loggers;
using System.Diagnostics;
using NLoggify.Logging.Config;

namespace NLoggify.Tests
{
    public class LoggerBehaviourTests
    {
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
