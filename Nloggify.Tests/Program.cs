using Nloggify.Tests.Examples;
using Nloggify.Tests.Examples.Simulations;
using NLoggify.Logging.Config;
using NLoggify.Logging.Loggers;
using NLoggify.Utils;

public class Program
{
    public static void Main(string[] args)
    {
        // Multi logger configuration
        LoggingConfig.ConfigureMultiLogger(LoggerType.Debug, LoggerType.Console, LoggerType.PlainText, LoggerType.JSON);

        LoggerType[] loggerTypes = { LoggerType.Multi };

        // Tests every supported type of logger
        foreach (LoggerType type in GenericUtils.GetEnumValues<LoggerType>())
        //foreach (LoggerType type in loggerTypes)
        {
            if (type == LoggerType.Multi)
            {
                int a = 5;
                a++;
            }

            // Configure the logger to use the current logger type to with the minimum log level set to Trace.
            LoggingConfig.Configure(LogLevel.Trace, type);

            // Current logger test
            Console.WriteLine($"===== {type} Logger Test Started =====\n");
            NLoggifyExample.Test(Logger.GetLogger());
            Console.WriteLine($"\n===== {type} Logger Test Completed =====\n\n\n");
        }
    }
}