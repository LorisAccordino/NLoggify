namespace NLoggify.Logging.Config
{
    public static class LogLevelColorConfig
    {
        // Dizionario che associa i livelli di log ai colori della console
        private static readonly Dictionary<LogLevel, ConsoleColor> _logLevelColors = new Dictionary<LogLevel, ConsoleColor>
        {
            { LogLevel.Trace, ConsoleColor.Gray },
            { LogLevel.Debug, ConsoleColor.Green },
            { LogLevel.Info, ConsoleColor.Cyan },
            { LogLevel.Warning, ConsoleColor.Yellow },
            { LogLevel.Error, ConsoleColor.DarkYellow },
            { LogLevel.Critical, ConsoleColor.Red },
            { LogLevel.Fatal, ConsoleColor.DarkRed }
        };

        /// <summary>
        /// Restituisce il colore associato a un determinato livello di log.
        /// </summary>
        public static ConsoleColor GetColorForLevel(LogLevel level)
        {
            return _logLevelColors.ContainsKey(level) ? _logLevelColors[level] : ConsoleColor.White;
        }

        /// <summary>
        /// Consente di configurare dinamicamente i colori per ogni livello di log.
        /// </summary>
        public static void ConfigureLogLevelColors(Dictionary<LogLevel, ConsoleColor> customColors)
        {
            foreach (var color in customColors)
            {
                if (_logLevelColors.ContainsKey(color.Key))
                {
                    _logLevelColors[color.Key] = color.Value;
                }
                else
                {
                    _logLevelColors.Add(color.Key, color.Value);
                }
            }
        }
    }
}
