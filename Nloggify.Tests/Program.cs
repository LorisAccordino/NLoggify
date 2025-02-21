
using Nloggify.Tests.Utils;
using NLoggify.Logging.Config;
using NLoggify.Logging.Loggers;

public class Program
{

    public static void Main(string[] args)
    {
        Console.WriteLine("===== ConsoleLogger Test Started =====\n");

        // Configure the logger to use ConsoleLogger with the minimum log level set to Trace.
        LoggingConfig.Configure(LogLevel.Trace, LoggerType.Console);
        var logger = Logger.GetLogger();

        logger.Log(LogLevel.Info, "Application started successfully.");

        // Start multiple concurrent operations to simulate a real system
        Task[] tasks =
        [
            Task.Run(() => Simulation.SimulateSystemInitialization(logger)),
            Task.Run(() => Simulation.SimulateDatabaseConnection(logger)),
            Task.Run(() => Simulation.SimulateDataProcessing(logger)),
            Task.Run(() => Simulation.SimulateConcurrentUserActivity(logger))
        ];

        // Wait for all tasks to complete
        Task.WaitAll(tasks);

        // Simulate an unexpected error and log the exception.
        try
        {
            Simulation.ThrowRandomException();
        }
        catch (Exception ex)
        {
            logger.Log(LogLevel.Error, $"An error occurred: {ex.Message}");
        }

        logger.Log(LogLevel.Info, "Application shutting down...");
        Console.WriteLine("\n===== ConsoleLogger Test Completed =====");
    }
}