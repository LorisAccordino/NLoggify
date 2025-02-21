namespace Nloggify.Tests.Utils
{
    /// <summary>
    /// A static helper class that provides utility methods for test simulations and logging.
    /// </summary>
    public static class TestHelper
    {
        private static readonly Random _random = new Random();

        /// <summary>
        /// Simulates a random delay within a specified range to introduce execution uncertainty.
        /// </summary>
        /// <param name="minMilliseconds">The minimum delay in milliseconds.</param>
        /// <param name="maxMilliseconds">The maximum delay in milliseconds.</param>
        /// <remarks>
        /// The delay duration is chosen randomly within the specified range.
        /// </remarks>
        /// <returns>The number of milliseconds <see cref="Thread.Sleep"/> slept for</returns>
        public static int RandomDelay(int minMilliseconds, int maxMilliseconds)
        {
            if (minMilliseconds > maxMilliseconds) throw new ArgumentException("minMilliseconds cannot be greater than maxMilliseconds.");

            int delay = _random.Next(minMilliseconds, maxMilliseconds + 1);
            Thread.Sleep(delay);
            return delay;
        }
    }
}