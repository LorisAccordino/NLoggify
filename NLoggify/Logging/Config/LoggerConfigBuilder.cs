using NLoggify.Logging.Loggers.Output;
using NLoggify.Logging.Loggers.Storage;
using NLoggify.Logging.Loggers;

namespace NLoggify.Logging.Config
{
    public class LoggerConfigBuilder
    {
        private HashSet<Type> _loggerTypes = new HashSet<Type>(); // Avoid duplicates

        private LoggerConfigBuilder() { }

        private LoggerConfigBuilder WriteToLogger<TLogger>(Func<TLogger> createLogger) where TLogger : Logger
        {
            if (_loggerTypes.Contains(typeof(TLogger)))
                throw new InvalidOperationException($"{typeof(TLogger).Name} is already registered!");

            _loggerTypes.Add(typeof(TLogger));
            LoggerWrapper.RegisterLogger(createLogger());
            return this;
        }

        public LoggerConfigBuilder WriteToDebugLogger(LoggerConfig? config = null)
        {
            return WriteToLogger(() => new DebugLogger(config ?? new LoggerConfig()));
        }

        public LoggerConfigBuilder WriteToConsoleLogger(ConsoleLoggerConfig? config = null)
        {
            return WriteToLogger(() => new ConsoleLogger(config ?? new ConsoleLoggerConfig()));
        }

        public LoggerConfigBuilder WriteToPlainTextFileLogger(FileLoggerConfig? config = null)
        {
            return WriteToLogger(() => new PlainTextLogger(config ?? new FileLoggerConfig()));
        }

        public LoggerConfigBuilder WriteToJsonFileLogger(FileLoggerConfig? config = null)
        {
            return WriteToLogger(() => new JsonLogger(config ?? new FileLoggerConfig()));
        }

        public Logger Build()
        {
            return LoggerWrapper.Instance;
        }
    }
}
