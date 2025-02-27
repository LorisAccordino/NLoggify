using Nloggify.Tests.Examples.Utils;
using NLoggify.Logging.Config.Enums;
using NLoggify.Logging.Loggers;
using System.Diagnostics.CodeAnalysis;

namespace Nloggify.Tests.Examples.Simulations
{
    /// <summary>
    /// Provides utility methods for simulating random failures, delays, and uncertainty in tests.
    /// </summary>
    [ExcludeFromCodeCoverage] // No reason to test it
    public static class GenericSimulations
    {
        private static readonly Random random = new Random();

        /// <summary>
        /// Simulates a system initialization process with random delays and potential warnings.
        /// </summary>
        /// <param name="logger">The logger instance used to log messages.</param>
        /// <param name="moduleCount">The number of system modules to initialize.</param>
        /// <param name="minDelayMilliseconds">The minimum random delay (in milliseconds) between each module initialization.</param>
        /// <param name="maxDelayMilliseconds">The maximum random delay (in milliseconds) between each module initialization.</param>
        /// <param name="warningChance">The probability (0-100) of a warning occurring during module initialization.</param>
        /// <remarks>
        /// This function simulates a system initialization process where each module is loaded with random delays.
        /// There is a configurable chance for a warning to be logged during each module loading.
        /// </remarks>
        public static void SimulateSystemInitialization(ILogger logger, int moduleCount = 5, int minDelayMilliseconds = 300, int maxDelayMilliseconds = 1000, int warningChance = 20)
        {
            if (warningChance < 0 || warningChance > 100) throw new ArgumentException("warningChance must be between 0 and 100.");

            logger.Log(LogLevel.Info, "Initializing system components...");

            for (int i = 1; i <= moduleCount; i++)
            {
                // Introduce a random delay for each module initialization
                TestHelper.RandomDelay(minDelayMilliseconds, maxDelayMilliseconds);

                // Simulate a warning based on the configurable chance
                if (random.Next(1, 101) <= warningChance) // Chance of warning occurring during the module load
                    logger.Log(LogLevel.Warning, $"Potential issue detected while loading module {i}/{moduleCount}.");

                // Log the successful loading of the module
                logger.Log(LogLevel.Debug, $"Module {i}/{moduleCount} loaded successfully.");
            }

            logger.Log(LogLevel.Info, "All modules initialized.");
        }


        /// <summary>
        /// Simulates a failure with a given probability.
        /// </summary>
        /// <param name="failureChance">The probability of failure (0 to 100).</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the random chance falls within the failure range.
        /// </exception>
        /// <remarks>
        /// If <paramref name="failureChance"/> is 30, there is a 30% chance an exception will be thrown.
        /// </remarks>
        public static void SimulateFailure(int failureChance)
        {
            if (failureChance < 0 || failureChance > 100) throw new ArgumentOutOfRangeException(nameof(failureChance), "Failure chance must be between 0 and 100.");
            if (random.Next(1, 101) <= failureChance) throw new InvalidOperationException("Simulated failure occurred.");
        }


        /// <summary>
        /// Simulates a database connection with a configurable chance of failure and retry logic.
        /// </summary>
        /// <param name="logger">The logger instance used to log messages.</param>
        /// <param name="initialFailureChance">The probability (0-100) of the first connection attempt failing.</param>
        /// <param name="retryFailureChance">The probability (0-100) of the second connection attempt failing after a retry.</param>
        /// <param name="initialDelayMin">The minimum delay (in milliseconds) before attempting to connect to the database.</param>
        /// <param name="initialDelayMax">The maximum delay (in milliseconds) before attempting to connect to the database.</param>
        /// <param name="retryDelayMin">The minimum delay (in milliseconds) before retrying the connection.</param>
        /// <param name="retryDelayMax">The maximum delay (in milliseconds) before retrying the connection.</param>
        /// <remarks>
        /// This function simulates a database connection attempt with a chance of failure. 
        /// If the first attempt fails, it retries once, with configurable delays and failure chances.
        /// </remarks>
        public static void SimulateDatabaseConnection(ILogger logger, int initialFailureChance = 30, int retryFailureChance = 50, int initialDelayMin = 1000, int initialDelayMax = 3000, int retryDelayMin = 1000, int retryDelayMax = 1500)
        {
            if (initialFailureChance < 0 || initialFailureChance > 100) throw new ArgumentException("initialFailureChance must be between 0 and 100.");
            if (retryFailureChance < 0 || retryFailureChance > 100) throw new ArgumentException("retryFailureChance must be between 0 and 100.");

            logger.Log(LogLevel.Info, "Connecting to database...");

            // Simulate initial connection delay
            TestHelper.RandomDelay(initialDelayMin, initialDelayMax);

            // Simulate initial connection attempt with configurable failure chance
            if (random.Next(1, 101) <= initialFailureChance) // Failure on first attempt based on the configured chance
            {
                logger.Log(LogLevel.Error, "Failed to connect to database! Retrying...");
                TestHelper.RandomDelay(retryDelayMin, retryDelayMax);

                // Simulate retry attempt with its own failure chance
                if (random.Next(1, 101) <= retryFailureChance) // Failure on retry based on the configured chance
                {
                    logger.Log(LogLevel.Critical, "Database connection could not be established.");
                    logger.LogException(LogLevel.Critical, () => throw new Exception("Database connection failure."));
                    
                }
                else
                    logger.Log(LogLevel.Info, "Database connection established successfully on retry.");
            }
            else
                logger.Log(LogLevel.Info, "Database connection established successfully.");
        }


        /// <summary>
        /// Simulates a data processing operation with configurable steps, delays, and warning frequency.
        /// </summary>
        /// <param name="logger">The logger instance used to log messages.</param>
        /// <param name="totalSteps">The total number of processing steps.</param>
        /// <param name="minDelay">The minimum delay in milliseconds between steps.</param>
        /// <param name="maxDelay">The maximum delay in milliseconds between steps.</param>
        /// <param name="warningFrequency">The frequency at which warnings occur (e.g., every N steps).</param>
        /// <remarks>
        /// The function simulates a data processing operation by logging each step, introducing 
        /// random delays, and logging warnings at the specified frequency.
        /// </remarks>
        public static void SimulateDataProcessing(ILogger logger, int totalSteps = 10, int minDelay = 200, int maxDelay = 700, int warningFrequency = 4)
        {
            if (minDelay > maxDelay) throw new ArgumentException("minDelay cannot be greater than maxDelay.");

            logger.Log(LogLevel.Info, "Starting background data processing...");

            for (int i = 1; i <= totalSteps; i++)
            {
                int delay = TestHelper.RandomDelay(minDelay, maxDelay);

                if (i % warningFrequency == 0) // Simulates a processing issue at the specified frequency
                    logger.Log(LogLevel.Warning, $"Data processing warning at step {i} (after {delay}ms delay).");
                else
                    logger.Log(LogLevel.Trace, $"Processing step {i}/{totalSteps} completed (after {delay}ms delay).");
            }

            logger.Log(LogLevel.Info, "Data processing finished successfully.");
        }
    }
}