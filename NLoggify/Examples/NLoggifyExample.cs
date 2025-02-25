using Nloggify.Tests.Examples.Simulations;
using NLoggify.Logging.Config.Enums;
using NLoggify.Logging.Loggers;

namespace Nloggify.Tests.Examples
{
    /// <summary>
    /// Provides utility methods as example to test the library features and see how they work.
    /// </summary>
    public static class NLoggifyExample
    {
        /// <summary>
        /// Test various scenarios on a specified logger
        /// </summary>
        /// <param name="logger">Logger to log on</param>
        /// <param name="fatalRisk">Where d'you wanna go? How much you wanna risk? ;)</param>
        public static void Test(ILogger logger, bool fatalRisk = false)
        {
            //logger.Log(LogLevel.Info, "Application started successfully.");

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
}
