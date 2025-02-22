using Nloggify.Tests.Utils.Simulations;
using NLoggify.Logging.Config;
using NLoggify.Logging.Loggers;
using NLoggify.Utils;

public class Program
{
    public static void Main(string[] args)
    {
        // Tests every supported type of logger
        foreach (LoggerType type in EnumUtil.GetValues<LoggerType>())
        {
            // Configure the logger to use the current logger type to with the minimum log level set to Trace.
            LoggingConfig.Configure(LogLevel.Trace, type);

            // Current logger test
            Console.WriteLine($"===== {type} Logger Test Started =====\n");
            Logger.GetLogger().LogException(LogLevel.Fatal, () => Test(Logger.GetLogger()), "AHHH, ECCEZIONEE");
            Console.WriteLine($"\n===== {type} Logger Test Completed =====\n\n\n");
        }

        return;

        // Configure the logger to use ConsoleLogger with the minimum log level set to Trace.
        LoggingConfig.Configure(LogLevel.Trace, LoggerType.Console);

        // ConsoleLogger test
        Console.WriteLine("===== ConsoleLogger Test Started =====\n");
        Test(Logger.GetLogger());
        Console.WriteLine("\n===== ConsoleLogger Test Completed =====");

        // DebugLogger test
        LoggingConfig.Configure(LogLevel.Trace, LoggerType.Debug); // Change configuration
        Console.WriteLine("===== ConsoleLogger Test Started =====\n");
        Test(Logger.GetLogger());
        Console.WriteLine("\n===== ConsoleLogger Test Completed =====");
    }

    public static void Test(ILogger logger, bool fatalRisk = false)
    {
        logger.Log(LogLevel.Info, "Application started successfully.");

        if (fatalRisk) ThreadWithFatalErrorSimulation.SimulateThreadWithFatalError(logger, 100000, 0.01f, 250);

        // Start multiple concurrent operations to simulate a real system
        Task[] tasks =
        [
            Task.Run(() => GenericSimulations.SimulateSystemInitialization(logger)),
            Task.Run(() => GenericSimulations.SimulateDatabaseConnection(logger)),
            Task.Run(() => GenericSimulations.SimulateDataProcessing(logger)),
            Task.Run(() => MultithreadSimulations.SimulateConcurrentUserActivity(logger))
        ];

        // Wait for all tasks to complete
        Task.WaitAll(tasks);

        // Simulate an unexpected error and log the exception.
        logger.LogException(LogLevel.Error, () => GenericSimulations.SimulateFailure(50), "An error occurred:");

        // End the simulation
        logger.Log(LogLevel.Info, "Application shutting down...");
    }
}