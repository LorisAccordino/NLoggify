using Nloggify.Tests.Examples;
using NLoggify.Examples.Simulations;
using NLoggify.Logging.Config;
using NLoggify.Logging.Config.Enums;
using NLoggify.Logging.Loggers;

public class Program
{
    public static void Main(string[] args)
    {
        string ascii_art = "\r\n  _   _ _                      _  __       \r\n | \\ | | |    ___   __ _  __ _(_)/ _|_   _ \r\n |  \\| | |   / _ \\ / _` |/ _` | | |_| | | |\r\n | |\\  | |__| (_) | (_| | (_| | |  _| |_| |\r\n |_| \\_|_____\\___/ \\__, |\\__, |_|_|  \\__, |\r\n                   |___/ |___/       |___/ \r\n";
        Console.WriteLine(ascii_art);

        //ConsoleLoggerConfig config = new ConsoleLoggerConfig();
        LoggerConfig config = new LoggerConfig();
        config.MinimumLogLevel = LogLevel.Debug;
        //config.UseColors = false;
        //config.FileNamePrefix = "ahahh";

        Logger.Configure().WriteToConsole(config);
        LoggerBuilder builder = Logger.Configure();

        //builder.WriteToConsole(); // Usa il default (ConsoleLoggerConfig)
        //builder.WriteToConsole(new LoggerConfig()); // Converte in ConsoleLoggerConfig
        //builder.WriteToConsole(new ConsoleLoggerConfig()); // Usa direttamente ConsoleLoggerConfig


        ILogger logger = Logger.GetLogger();

        //NLoggifyExamples.Test(logger);
        //NLoggifyExamples.StressTest(logger);
        //NLoggifyExamples.BufferedTest(logger);
        IntensiveSimulations.CpuStressTestWithLogging(logger, 30, 100);
    }
}
