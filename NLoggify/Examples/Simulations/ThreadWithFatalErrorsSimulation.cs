using System.Diagnostics;
using NLoggify.Logging.Config.Enums;
using NLoggify.Logging.Loggers;

namespace Nloggify.Tests.Examples.Simulations
{
    /// <summary>
    /// Simulates a thread that can generate fatal errors with an increasingly probability over the time.
    /// The errors escalation has a logarithmic trend.
    /// </summary>
    public static class ThreadWithFatalErrorSimulation
    {
        private static readonly Random _random = new Random();

        /// <summary>
        /// Simulate a thread that risks of generating a fatal error with increasingly probability over the time.
        /// The probability of causing errors raises following a logarithmic curve.
        /// </summary>
        /// <param name="logger">The used logger to log the messages.</param>
        /// <param name="maxDurationMilliseconds">The max duration in milliseconds for the thread simulation.</param>
        /// <param name="initialFailureProbability">The initial probability of encountering fatal error. (Default is 0.01)</param>
        /// <param name="failureGrowthFactor">The growth factor of error probability (Default is 1000)</param>
        public static async Task SimulateThreadWithFatalError(ILogger logger, int maxDurationMilliseconds, double initialFailureProbability = 0.01, double failureGrowthFactor = 1000)
        {
            var startTime = DateTime.Now;
            double failureProbability = initialFailureProbability;

            // Create a thread that simulates a continuous operation
            await Task.Run(async () =>
            {
                int elapsedTime = 0;

                while (elapsedTime < maxDurationMilliseconds)
                {
                    elapsedTime = (int)(DateTime.Now - startTime).TotalMilliseconds;

                    // Calculate the increasingly error probability logarithmically
                    failureProbability = CalculateFailureProbability(elapsedTime, failureGrowthFactor);
                    Debug.WriteLine(failureProbability);

                    // Message log based on the probability
                    LogMessageBasedOnProbability(logger, failureProbability);

                    // If the probability is high enough, generate a fatal error
                    //if (_random.NextDouble() < failureProbability)
                    if ((_random.NextDouble() < failureProbability / 5 && failureProbability >= 0.2f) || failureProbability >= 1.0f)
                    {
                        logger.Log(LogLevel.Fatal, $"CRASH! Fatal error at {elapsedTime} ms. The system is shutting down!");
                        logger.LogException(LogLevel.Fatal, () => throw new Exception("Fatal error: the system has been compromised."), "The thread has ended due to a fatal error:");
                        Environment.Exit(-1);
                    }

                    // Asynchronous delay to avoid main thread blocking
                    await Task.Delay(100);
                }
                logger.Log(LogLevel.Info, "The simulation ended without fatal errors.");
            });
        }

        /// <summary>
        /// Calculate the fatal error probability based on elapsed time, following a logarithmic trend.
        /// </summary>
        /// <param name="elapsedTime">The elapsed time in milliseconds.</param>
        /// <param name="growthFactor">The growth factor that controls the growth of probability.</param>
        /// <returns>The calculated error probability.</returns>
        private static double CalculateFailureProbability(int elapsedTime, double growthFactor)
        {
            return Math.Log(elapsedTime + 1) * (growthFactor / 10000);
        }

        /// <summary>
        /// Log the messages based on error probability.
        /// </summary>
        /// <param name="logger">The used logger to log messages.</param>
        /// <param name="failureProbability">The calculated probability error.</param>
        private static void LogMessageBasedOnProbability(ILogger logger, double failureProbability)
        {
            if (failureProbability < 0.05)
                logger.Log(LogLevel.Trace, "System is stable. No issues encountered.");
            else if (failureProbability < 0.1)
                logger.Log(LogLevel.Info, "Minor issues encountered.");
            else if (failureProbability < 0.2)
                logger.Log(LogLevel.Warning, "Anomalies detected.");
            else if (failureProbability < 0.5)
                logger.Log(LogLevel.Error, "Serious problems in progress.");
        }
    }
}