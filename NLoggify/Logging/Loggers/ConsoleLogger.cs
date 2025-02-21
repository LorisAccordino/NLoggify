namespace NLoggify.Logging.Loggers
{
    internal class ConsoleLogger : Logger
    {
        protected override void WriteLog(LogLevel level, string message)
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
