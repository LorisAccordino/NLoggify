using NLoggify.Logging.Config;

namespace NLoggify.Logging.Loggers.Storage
{
    internal class FileLogger : Logger
    {
        protected override void WriteLog(LogLevel level, string message, string timestamp)
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
