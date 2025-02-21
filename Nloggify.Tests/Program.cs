
using NLoggify.Logging.Config;
using NLoggify.Logging.Loggers;

public class Program
{
    private static readonly Random _random = new Random();

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
            Task.Run(() => SimulateSystemInitialization(logger)),
            Task.Run(() => SimulateDatabaseConnection(logger)),
            Task.Run(() => SimulateDataProcessing(logger)),
            Task.Run(() => SimulateConcurrentUserActivity(logger))
        ];

        // Wait for all tasks to complete
        Task.WaitAll(tasks);

        // Simulate an unexpected error and log the exception.
        try
        {
            ThrowRandomException();
        }
        catch (Exception ex)
        {
            logger.Log(LogLevel.Error, $"An error occurred: {ex.Message}");
        }

        logger.Log(LogLevel.Info, "Application shutting down...");
        Console.WriteLine("\n===== ConsoleLogger Test Completed =====");
    }

    /// <summary>
    /// Simulates a system initialization process with random delays and potential warnings.
    /// </summary>
    private static void SimulateSystemInitialization(ILogger logger)
    {
        logger.Log(LogLevel.Info, "Initializing system components...");
        for (int i = 1; i <= 5; i++)
        {
            Thread.Sleep(_random.Next(300, 1000)); // Random delay
            if (_random.Next(1, 10) <= 2) // 20% chance of warning
            {
                logger.Log(LogLevel.Warning, $"Potential issue detected while loading module {i}/5.");
            }
            logger.Log(LogLevel.Debug, $"Module {i}/5 loaded successfully.");
        }
        logger.Log(LogLevel.Info, "All modules initialized.");
    }

    /// <summary>
    /// Simulates a database connection with a chance of failure.
    /// If the connection fails, it retries once before throwing an exception.
    /// </summary>
    private static void SimulateDatabaseConnection(ILogger logger)
    {
        logger.Log(LogLevel.Info, "Connecting to database...");
        Thread.Sleep(_random.Next(1000, 3000)); // Simulated database delay

        if (_random.Next(1, 100) <= 30) // 30% chance of failure
        {
            logger.Log(LogLevel.Error, "Failed to connect to database! Retrying...");
            Thread.Sleep(1500);

            if (_random.Next(1, 100) <= 50) // 50% chance of a second failure
            {
                logger.Log(LogLevel.Critical, "Database connection could not be established.");
                throw new Exception("Database connection failure.");
            }
            else
            {
                logger.Log(LogLevel.Info, "Database connection established successfully on retry.");
            }
        }
        else
        {
            logger.Log(LogLevel.Info, "Database connection established successfully.");
        }
    }

    /// <summary>
    /// Simulates a data processing operation with occasional warnings and randomized delays.
    /// </summary>
    private static void SimulateDataProcessing(ILogger logger)
    {
        logger.Log(LogLevel.Info, "Starting background data processing...");
        for (int i = 1; i <= 10; i++)
        {
            Thread.Sleep(_random.Next(200, 700));
            if (i % 4 == 0) // Simulates a processing issue every 4th step
            {
                logger.Log(LogLevel.Warning, $"Data processing warning at step {i}.");
            }
            else
            {
                logger.Log(LogLevel.Trace, $"Processing step {i}/10 completed.");
            }
        }
        logger.Log(LogLevel.Info, "Data processing finished successfully.");
    }

    /// <summary>
    /// Simulates multiple concurrent users logging messages.
    /// </summary>
    private static void SimulateConcurrentUserActivity(ILogger logger)
    {
        logger.Log(LogLevel.Info, "Simulating concurrent user activity...");
        Parallel.For(0, 5, userId =>
        {
            for (int action = 1; action <= 3; action++)
            {
                Thread.Sleep(_random.Next(500, 1500));
                logger.Log(LogLevel.Debug, $"User {userId} performed action {action}.");
            }
        });
        logger.Log(LogLevel.Info, "All user actions completed.");
    }

    /// <summary>
    /// Randomly throws an exception 25% of the time to simulate system failures.
    /// </summary>
    private static void ThrowRandomException()
    {
        if (_random.Next(1, 4) == 2) // 25% probability of throwing an exception
        {
            throw new InvalidOperationException("Simulated system failure.");
        }
    }
}