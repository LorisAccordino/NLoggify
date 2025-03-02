using Nloggify.Tests.Examples;
using NLoggify.Examples.Simulations;
using NLoggify.Logging.Config;
using NLoggify.Logging.Config.Enums;
using NLoggify.Logging.Core;
using NLoggify.Logging.Loggers;

public class Program
{
    public static void Main(string[] args)
    {
        string ascii_art = "\r\n  _   _ _                      _  __       \r\n | \\ | | |    ___   __ _  __ _(_)/ _|_   _ \r\n |  \\| | |   / _ \\ / _` |/ _` | | |_| | | |\r\n | |\\  | |__| (_) | (_| | (_| | |  _| |_| |\r\n |_| \\_|_____\\___/ \\__, |\\__, |_|_|  \\__, |\r\n                   |___/ |___/       |___/ \r\n";
        Console.WriteLine(ascii_art);

        LoggerConfig config = new LoggerConfig();
        config.MinimumLogLevel = LogLevel.Debug;

        ConsoleLoggerConfig consoleConfig = new ConsoleLoggerConfig(config);
        //consoleConfig.UseColors = false;

        FileLoggerConfig fileLoggerConfig = new FileLoggerConfig(config);
        //fileLoggerConfig.FileNamePrefix = "output";

        LoggerManager.Configure()
            //.WriteToDebug(config)
            .WriteToConsole(consoleConfig)
            .WriteToPlainTextFile(fileLoggerConfig)
            .WriteToJsonFile(fileLoggerConfig)
            .Build();

        ILogger logger = LoggerManager.GetLogger();

        NLoggifyExamples.Test(logger, true);
        /*NLoggifyExamples.StressTest(logger);
        NLoggifyExamples.BufferedTest(logger);
        IntensiveSimulations.CpuStressTestWithLogging(logger, 30, 100);*/
    }
}
