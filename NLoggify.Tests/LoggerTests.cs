using NLoggify.Logging.Loggers;
using NLoggify.Logging;

namespace NLoggify.Tests
{
    public class LoggerTests
    {
        [Fact]
        public void LoggerInstance_ShouldNotBeNull_AfterConfiguration()
        {
            // Configura il logger
            LoggingConfig.ConfigureLogging(LogLevel.Info, LoggerType.Console);

            // Ottieni l'istanza
            var logger = Logger.GetInstance();

            // Verifica che l'istanza non sia null
            Assert.NotNull(logger);
        }
    }
}