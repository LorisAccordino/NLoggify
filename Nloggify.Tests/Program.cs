using Nloggify.Tests.Utils.Simulations;
using NLoggify.Logging.Config;
using NLoggify.Logging.Loggers;

public class Program
{
    public static void Main(string[] args)
    {
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

    public static void Test(ILogger logger)
    {
        logger.Log(LogLevel.Info, "Application started successfully.");

        ThreadWithFatalErrorSimulation.SimulateThreadWithFatalError(logger, 100000);

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
        try
        {
            GenericSimulations.SimulateFailure(50);
        }
        catch (Exception ex)
        {
            logger.Log(LogLevel.Error, $"An error occurred: {ex.Message}");
        }

        logger.Log(LogLevel.Info, "Application shutting down...");
    }
}