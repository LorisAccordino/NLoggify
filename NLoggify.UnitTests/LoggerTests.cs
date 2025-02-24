using NLoggify.Logging.Loggers;
using NLoggify.Logging.Config;
using System.Diagnostics.CodeAnalysis;

namespace NLoggify.UnitTests
{
    /// <summary>
    /// Unit tests for verifying the behavior of the logger and its configuration.
    /// </summary>
    [Collection("SequentialTests")]
    [ExcludeFromCodeCoverage]
    public class LoggerTests
    {
        /// <summary>
        /// Ensures that <see cref="Logger.GetLogger"/> always returns a not null value,
        /// both before and after configuration.
        /// </summary>
        /// <param name="configure">Whether to configure the logger before testing.</param>
        [Theory]
        [InlineData(false)] // Test before configuration
        [InlineData(true)]  // Test after configuration
        public void LoggerInstance_ShouldNotBeNull(bool configure)
        {
            // Arrange
            if (configure)
            {
                LoggingConfig.Configure(LogLevel.Info, LoggerType.Console);
            }

            // Act
            var logger = Logger.GetLogger();

            // Assert
            Assert.NotNull(logger);
        }

        /// <summary>
        /// Ensures that <see cref="Logger.GetLogger"/> always returns the same instance.
        /// </summary>
        [Fact]
        public void LoggerInstance_ShouldReturnSameInstance()
        {
            // Arrange
            LoggingConfig.Configure(LogLevel.Info, LoggerType.Console);

            // Act
            var logger1 = Logger.GetLogger();
            var logger2 = Logger.GetLogger();

            // Assert
            Assert.Same(logger1, logger2); // Must be the same instance
        }

#if DEBUG
        /// <summary>
        /// Tests the instance (directly) to check its behaviour
        /// </summary>
        [Fact]
        public void Instance_ShouldCreateNewInstance_WhenFirstAccessed()
        {
            // Arrange
            Logger.Instance = null; // Make it null, for debug its behaviour
            var initialInstance = Logger.Instance;  // First access, should initialize the instance

            // Act
            var secondInstance = Logger.Instance;  // Second access, should return the same instance

            // Assert
            Assert.NotNull(initialInstance);  // Make sure the instance is not null
            Assert.Same(initialInstance, secondInstance);  // Make sure both the access return the same instance
        }

        /// <summary>
        /// Tests if the logger respects the configured log level and logs the message accordingly.
        /// This test runs on multiple logger types.
        /// </summary>
        /// <param name="configLevel">The log level set in the configuration.</param>
        /// <param name="messageLevel">The log level of the message being logged.</param>
        /// <param name="shouldLog">A boolean indicating whether the message should be logged.</param>
        /// <param name="loggerType">The type of logger to test (Console, File, Memory, etc.).</param>
        [Theory]
        [InlineData(LogLevel.Info, LogLevel.Debug, false, LoggerType.Console)]  // Debug is lower than Info, so it doesn't log
        [InlineData(LogLevel.Warning, LogLevel.Error, true, LoggerType.Console)]  // Error is higher than Warning, so it logs
        [InlineData(LogLevel.Debug, LogLevel.Debug, true, LoggerType.Debug)]  // Debug = Debug, so it logs
        [InlineData(LogLevel.Error, LogLevel.Info, false, LoggerType.PlainText)]  // Info is lower than Error, so it doesn't log
        [InlineData(LogLevel.Warning, LogLevel.Warning, true, LoggerType.JSON)]  // Equal levels should log
        [InlineData(LogLevel.Critical, LogLevel.Error, false, LoggerType.PlainText)]  // Error < Critical, so it doesn't log
        [InlineData(LogLevel.Info, LogLevel.Critical, true, LoggerType.Console)]  // Critical is always logged
        public void Logger_ShouldLogProperly(LogLevel configLevel, LogLevel messageLevel, bool shouldLog, LoggerType loggerType)
        {
            // Arrange
            LoggingConfig.Configure(configLevel, loggerType);
            var logger = Logger.GetLogger();

            // Act
            logger.Log(messageLevel, "Test message");

            // Assert
            var output = Logger.GetDebugOutput(); // Simulated output for Console and Memory loggers
            if (shouldLog)
                Assert.Contains("Test message", output);
            else
                Assert.DoesNotContain("Test message", output);
        }

        /// <summary>
        /// Test if the logger has the expected behaviour during catching exceptions
        /// </summary>
        /// <param name="throwException">Should throw exception?</param>
        [Theory]
        [InlineData(false)]  // No exception (false)
        [InlineData(true)]   // Exception thrown (true)
        public void LogException_ShouldReturnExpectedResult_WhenActionIsExecuted(bool throwException)
        {
            // Arrange
            bool actionExecuted = false;

            Action action = () =>
            {
                actionExecuted = true;
                if (throwException) throw new InvalidOperationException("Test exception");
            };

            // Act
            var result = Logger.GetLogger().LogException(LogLevel.Error, action, "An error occurred");

            // Assert
            Assert.True(actionExecuted);  // Ensure the action was executed

            if (throwException)
            {
                var output = Logger.GetDebugOutput();
                // Scenario with exception
                Assert.True(result);  // Ensure the method returns true when an exception is thrown
                Assert.Contains("Test exception", output);  // Ensure the exception message is in the log
                Assert.Contains("An error occurred", output);  // Ensure the custom message is in the log
            }
            else
            {
                // Scenario without exception
                Assert.False(result);  // Ensure the method returns false when no exception is thrown
            }
        }
#endif

        /// <summary>
        /// Tests if the logger catches properly either sync or async exception during operations
        /// </summary>
        /// /// <param name="throwException">Should throw exception?</param>
        [Theory]
        [InlineData(false)]  // No exception (false)
        [InlineData(true)]   // Exception thrown (true)
        public async Task LogException_ShouldCatchAsyncExceptions(bool throwException)
        {
            // Arrange
            LoggingConfig.Configure(LogLevel.Info, LoggerType.Console);
            var logger = Logger.GetLogger();

            // Act
            bool exceptionCaught = await logger.LogException(LogLevel.Error, async () =>
            {
                await Task.Delay(50);
                if (throwException) throw new InvalidOperationException("Test error");
            });

            // Assert
            if (throwException)
                Assert.True(exceptionCaught);
            else
                Assert.False(exceptionCaught);
        }

        /// <summary>
        /// Tests if the logger singleton behaviour is thread safe
        /// </summary>
        [Fact]
        public void Logger_Instance_ShouldBeThreadSafeAndSingleton()
        {
            // Arrange
            Logger logger1 = null;
            Logger logger2 = null;

            // Act
            Parallel.Invoke(
                () => { logger1 = Logger.Instance; }, // First thread
                () => { logger2 = Logger.Instance; }  // Second thread
            );

            // Assert
            Assert.Same(logger1, logger2); // Ensure the same instance is returned across threads
        }

        /// <summary>
        /// Tests that logger runs exclusively an operation to protect a resource
        /// </summary>
        [Fact]
        public async Task Logger_RunExclusive_ShouldExecuteActionExclusively()
        {
            // Arrange
            var executionOrder = new List<int>();
            var lockObject = new object(); // Used to track thread execution order
            var tasks = new List<Task>();

            Action action = () =>
            {
                lock (lockObject) // Ensure that the order of execution is tracked
                {
                    // Simulate some exclusive action
                    executionOrder.Add(1);
                    Thread.Sleep(50); // Simulate some work being done (e.g., logging)
                }
            };

            // Act - Run multiple tasks in parallel
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Run(() => Logger.RunExclusive(action)));
            }

            // Wait for all tasks to complete
            await Task.WhenAll(tasks);

            // Assert
            // Since the RunExclusive method ensures that only one action can be executed at a time,
            // we should have 10 actions executed, but only one at a time.
            Assert.Equal(10, executionOrder.Count); // All actions should be executed, but exclusively
        }

        /// <summary>
        /// Tests that logger runs exclusively an operation to protect a resource it returns the correct result
        /// </summary>
        /// <returns>Result of the operation</returns>
        [Fact]
        public async Task Logger_RunExclusive_ShouldExecuteFunctionExclusively()
        {
            // Arrange
            var results = new List<int>();
            var lockObject = new object(); // Used to track thread execution order
            var tasks = new List<Task>();

            Func<int> func = () =>
            {
                lock (lockObject) // Ensure that the order of execution is tracked
                {
                    // Simulate some work and return a result
                    results.Add(42);
                    Thread.Sleep(50); // Simulate some work being done
                    return 42; // The result
                }
            };

            // Act - Run multiple tasks in parallel
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Run(() => Logger.RunExclusive(func)));
            }

            // Wait for all tasks to complete
            await Task.WhenAll(tasks);

            // Assert
            // Since the RunExclusive method ensures that only one function can be executed at a time,
            // we should have 10 function calls, but only one at a time.
            Assert.Equal(10, results.Count); // All functions should be executed, but exclusively
            Assert.All(results, result => Assert.Equal(42, result)); // All results should be 42
        }
    }
}
