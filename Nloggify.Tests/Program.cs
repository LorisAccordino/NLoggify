using Nloggify.Tests.Examples;
using NLoggify.Logging.Config;
using NLoggify.Logging.Loggers;

public class Program
{
    public static void Main(string[] args)
    {
        // Multi logger configuration
        LoggingConfig.ConfigureMultiLogger(LoggerType.Console, LoggerType.PlainText);

        // Tests every supported type of logger
        foreach (LoggerType type in Enum.GetValues<LoggerType>())
        {
            // Configure the logger to use the current logger type to with the minimum log level set to Trace.
            LoggingConfig.Configure(LogLevel.Trace, type);

            // Current logger test
            Console.WriteLine($"===== {type} Logger Test Started =====\n");
            NLoggifyExample.Test(Logger.GetLogger());
            Console.WriteLine($"\n===== {type} Logger Test Completed =====\n\n\n");
        }
    }
}