using NLoggify.Logging.Config;

namespace NLoggify.Logging.Loggers
{
    internal class FileLogger : Logger
    {
        protected override void WriteLog(LogLevel level, string message, DateTime timestamp)
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
