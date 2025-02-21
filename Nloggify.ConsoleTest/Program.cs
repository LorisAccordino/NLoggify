using NLoggify.Logging.Config;
using NLoggify.Logging.Loggers;

public class Program
{
    public static void Main(string[] args)
    {
        // Arrange
        LoggingConfig.Configure(LogLevel.Trace, LoggerType.Console);
        var logger = Logger.GetLogger();

        logger.Log(LogLevel.Trace, "Test message");
        logger.Log(LogLevel.Debug, "Test message");
        logger.Log(LogLevel.Info, "Test message");
        logger.Log(LogLevel.Warning, "Test message");
        logger.Log(LogLevel.Error, "Test message");
        logger.Log(LogLevel.Critical, "Test message");
        logger.Log(LogLevel.Fatal, "Test message");
    }
}