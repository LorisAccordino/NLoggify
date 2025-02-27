using System.Diagnostics.CodeAnalysis;

namespace Nloggify.Tests.Examples.Utils
{
    /// <summary>
    /// A static helper class that provides utility methods for test simulations and logging.
    /// </summary>
    [ExcludeFromCodeCoverage] // No reason to test it
    public static class TestHelper
    {
        private static readonly Random random = new Random();

        /// <summary>
        /// Simulates a random delay within a specified range to introduce execution uncertainty.
        /// </summary>
        /// <param name="minMilliseconds">The minimum delay in milliseconds.</param>
        /// <param name="maxMilliseconds">The maximum delay in milliseconds.</param>
        /// <remarks>
        /// The delay duration is chosen randomly within the specified range.
        /// </remarks>
        /// <returns>The number of milliseconds <see cref="Thread.Sleep(int)"/> slept for</returns>
        public static int RandomDelay(int minMilliseconds, int maxMilliseconds)
        {
            if (minMilliseconds > maxMilliseconds) throw new ArgumentException("minMilliseconds cannot be greater than maxMilliseconds.");

            int delay = random.Next(minMilliseconds, maxMilliseconds + 1);
            Thread.Sleep(delay);
            return delay;
        }
    }
}