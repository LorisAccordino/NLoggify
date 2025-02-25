using NLoggify.Logging.Config;
using NLoggify.Logging.Loggers;
using System.Diagnostics;
using System.Text;

namespace NLoggify.Examples.Simulations
{
    /// <summary>
    /// Provides utility methods for simulating intensive operations like CPU stress tests, intensive I/O operations, buffered logging, multithread stress etc.
    /// </summary>
    public static class IntensiveSimulations
    {
        /// <summary>
        /// Simulates massive log generation in a sequential manner.
        /// </summary>
        /// <param name="logger">Logger to log on</param>
        /// <param name="iterations">The number of logs to generate.</param>
        public static void MassiveSingleThreadedTest(ILogger logger, int iterations)
        {
            logger.Log(LogLevel.Info, $"Starting stress test with {iterations} iterations.");
            Stopwatch sw = Stopwatch.StartNew();

            for (int i = 0; i < iterations; i++)
            {
                logger.Log(LogLevel.Debug, $"Stress test iteration #{i}");
            }

            sw.Stop();
            logger.Log(LogLevel.Info, $"Stress test completed in {sw.ElapsedMilliseconds} ms");
        }

        /// <summary>
        /// Simulates high-volume logging using multiple threads.
        /// </summary>
        /// <param name="logger">Logger to log on</param>
        /// <param name="threadCount">The number of concurrent threads.</param>
        /// <param name="iterationsPerThread">The number of logs each thread generates.</param>
        public static void MassiveMultiThreadedTest(ILogger logger, int threadCount, int iterationsPerThread)
        {
            logger.Log(LogLevel.Info, $"Starting multi-threaded logging with {threadCount} threads, {iterationsPerThread} logs per thread.");

            Parallel.For(0, threadCount, threadId =>
            {
                for (int i = 0; i < iterationsPerThread; i++)
                {
                    logger.Log(LogLevel.Debug, $"Multithread intensive iteration {i}");
                }
            });

            logger.Log(LogLevel.Info, "Multi-threaded logging completed.");
        }

        /// <summary>
        /// Simulates log entries with random log levels.
        /// </summary>
        /// <param name="logger">Logger to log on</param>
        /// <param name="iterations">The number of logs to generate.</param>
        public static void RunRandomLogTest(ILogger logger, int iterations)
        {
            logger.Log(LogLevel.Info, $"Starting random log simulation with {iterations} iterations.");
            Random rnd = new Random();
            LogLevel[] levels = { LogLevel.Debug, LogLevel.Info, LogLevel.Warning, LogLevel.Error, LogLevel.Critical };

            for (int i = 0; i < iterations; i++)
            {
                LogLevel level = levels[rnd.Next(levels.Length)];
                logger.Log(level, $"Random log {i}");
            }

            logger.Log(LogLevel.Info, "Random log simulation completed.");
        }

        /// <summary>
        /// Simulates logging with buffering to reduce I/O operations.
        /// </summary>
        /// <param name="logger">Logger to log on</param>
        /// <param name="iterations">The number of logs to generate.</param>
        public static void RunBufferedLoggingTest(ILogger logger, int iterations)
        {
            logger.Log(LogLevel.Info, $"Starting buffered logging simulation with {iterations} iterations.");
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < iterations; i++)
            {
                sb.AppendLine($"Buffered log {i}");
            }

            logger.Log(LogLevel.Info, sb.ToString());
            logger.Log(LogLevel.Info, "Buffered logging simulation completed.");
        }



        /*** CPU STRESS TEST ***/

        /// <summary>
        /// Runs a CPU stress test while logging CPU usage and criticality levels.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="durationSeconds">Duration of the stress test.</param>
        /// <param name="logIntervalMilliseconds">Interval (in milliseconds) for logging CPU usage.</param>
        public static void CpuStressTestWithLogging(ILogger logger, int durationSeconds, int logIntervalMilliseconds)
        {
            int coreCount = Environment.ProcessorCount;
            logger.Log(LogLevel.Info, $"Starting CPU stress test on {coreCount} threads for {durationSeconds} seconds...");

            using CancellationTokenSource cts = new();
            CancellationToken token = cts.Token;

            // Start CPU monitoring/logging task on a separate thread
            Thread monitorThread = new Thread(() =>
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                while (stopwatch.Elapsed.TotalSeconds < durationSeconds && !token.IsCancellationRequested)
                {
                    // Log CPU usage every second (adjust interval as needed)
                    double totalCpuUsage = GetCpuUsage();
                    double processCpuUsage = GetProcessCpuUsage();
                    LogCpuCriticality(logger, totalCpuUsage, processCpuUsage);
                    Thread.Sleep(logIntervalMilliseconds); // Log every second
                }
            });

            // Start the monitoring thread
            monitorThread.Priority = ThreadPriority.Highest;
            monitorThread.Start();
            

            // Start CPU-intensive tasks after 1/6 of the total time
            Task.Delay(TimeSpan.FromSeconds(durationSeconds / 6.0)).Wait();

            
            // Start CPU stress task for 2/3 of the total time
            Task[] tasks = new Task[coreCount];
            for (int i = 0; i < coreCount; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    double value = 1.0001;
                    Stopwatch stressStopwatch = Stopwatch.StartNew();
                    while (stressStopwatch.Elapsed.TotalSeconds < (durationSeconds * 2 / 3) && !token.IsCancellationRequested)
                    {
                        value = Math.Sqrt(value) * Math.Sqrt(value); // Stress CPU with a dummy calculation
                    }
                }, token);

                if (i % 2 == 0) Task.Delay(TimeSpan.FromSeconds(durationSeconds / (coreCount / 2)));
            }

            // Wait for test duration and then cancel everything
            //Task.Delay(TimeSpan.FromSeconds(durationSeconds)).ContinueWith(_ => cts.Cancel()).Wait();

            try
            {
                // Wait for stress test to finish
                Task.WaitAll(tasks);

                // Wait 1/6 of the time for the post-stress monitoring
                Task.Delay(TimeSpan.FromSeconds(durationSeconds / 6.0)).Wait();

                // Cancel the monitoring and finish the test
                cts.Cancel();
                monitorThread.Join();
            }
            catch (AggregateException ex)
            {
                // Ignore only TaskCanceledException, throw other exceptions
                ex.Handle(e => e is TaskCanceledException);
            }

            logger.Log(LogLevel.Info, "CPU stress test completed.");
        }

        /// <summary>
        /// Retrieves the current CPU usage percentage.
        /// </summary>
        private static double GetCpuUsage()
        {
            using PerformanceCounter cpuCounter = new("Processor", "% Processor Time", "_Total");
            cpuCounter.NextValue();
            Thread.Sleep(500); // Let the counter refresh
            return cpuCounter.NextValue();
        }

        /// <summary>
        /// Retrieves the CPU usage percentage of the current process
        /// </summary>
        /// <returns></returns>
        private static double GetProcessCpuUsage()
        {
            using PerformanceCounter processCounter = new("Process", "% Processor Time", Process.GetCurrentProcess().ProcessName);
            return processCounter.NextValue();
        }

        /// <summary>
        /// Logs CPU criticality level based on CPU usage percentage.
        /// </summary>
        private static void LogCpuCriticality(ILogger logger, double totalCpuUsage, double processCpuUsage)
        {
            LogLevel level = totalCpuUsage switch
            {
                < 30 => LogLevel.Info,
                < 60 => LogLevel.Warning,
                < 85 => LogLevel.Error,
                _ => LogLevel.Critical
            };

            logger.Log(level, $"CPU Usage: {totalCpuUsage:F2}% (Process: {processCpuUsage:F2}%)");
        }
    }
}
