using Nloggify.Tests.Examples.Utils;
using NLoggify.Logging.Config.Enums;
using NLoggify.Logging.Loggers;

namespace Nloggify.Tests.Examples.Simulations
{
    /// <summary>
    /// A class that simulates the activity of concurrent users performing actions.
    /// Contains methods to simulate multiple users performing actions with random delays in parallel.
    /// </summary>
    public static class MultithreadSimulations
    {
        /// <summary>
        /// Simulates multiple concurrent users performing actions and logging messages.
        /// </summary>
        /// <param name="logger">The logger instance used for logging messages.</param>
        /// <param name="userCount">The number of concurrent users to simulate.</param>
        /// <param name="actionsPerUser">The number of actions each user will perform.</param>
        /// <param name="minDelayMs">The minimum delay (in milliseconds) between actions.</param>
        /// <param name="maxDelayMs">The maximum delay (in milliseconds) between actions.</param>
        public static void SimulateConcurrentUserActivity(ILogger logger, int userCount = 5, int actionsPerUser = 3, int minDelayMs = 500, int maxDelayMs = 1500)
        {
            if (userCount <= 0) throw new ArgumentOutOfRangeException(nameof(userCount), "User count must be greater than zero.");
            if (actionsPerUser <= 0) throw new ArgumentOutOfRangeException(nameof(actionsPerUser), "Actions per user must be greater than zero.");
            if (minDelayMs < 0 || maxDelayMs < 0 || minDelayMs > maxDelayMs) throw new ArgumentException("Invalid delay range. Ensure minDelayMs is non-negative and not greater than maxDelayMs.");

            logger.Log(LogLevel.Info, $"Simulating {userCount} concurrent users, each performing {actionsPerUser} actions...");

            Parallel.For(0, userCount, userId =>
            {
                for (int action = 1; action <= actionsPerUser; action++)
                {
                    int delay = TestHelper.RandomDelay(minDelayMs, maxDelayMs);
                    logger.Log(LogLevel.Debug, $"User {userId} performed action {action} after {delay} ms.");
                }
            });

            logger.Log(LogLevel.Info, "All user actions completed.");
        }
    }
}