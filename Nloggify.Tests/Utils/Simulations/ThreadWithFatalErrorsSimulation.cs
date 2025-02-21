using NLoggify.Logging.Loggers;
using NLoggify.Logging.Config;

namespace Nloggify.Tests.Utils.Simulations
{
    /// <summary>
    /// Simula un thread che può generare errori fatali con una probabilità crescente nel tempo.
    /// L'escalation degli errori segue un modello logaritmico.
    /// </summary>
    public static class ThreadWithFatalErrorSimulation
    {
        private static readonly Random _random = new Random();

        /// <summary>
        /// Simula un thread che rischia di generare un errore fatale con probabilità crescente nel tempo.
        /// La probabilità di errore aumenta secondo una curva logaritmica.
        /// </summary>
        /// <param name="logger">Il logger utilizzato per registrare i messaggi di log.</param>
        /// <param name="maxDurationMilliseconds">La durata massima in millisecondi per la simulazione del thread.</param>
        /// <param name="initialFailureProbability">La probabilità iniziale di errore fatale. (Default è 0.01)</param>
        /// <param name="failureGrowthFactor">Il fattore di crescita della probabilità di errore (Default è 1000)</param>
        public static void SimulateThreadWithFatalError(ILogger logger, int maxDurationMilliseconds, double initialFailureProbability = 0.01, double failureGrowthFactor = 1000)
        {
            var startTime = DateTime.Now;
            double failureProbability = initialFailureProbability;

            // Crea il thread che simula un'operazione continua
            Thread simulationThread = new Thread(() =>
            {
                try
                {
                    int elapsedTime = 0;

                    while (elapsedTime < maxDurationMilliseconds)
                    {
                        elapsedTime = (int)(DateTime.Now - startTime).TotalMilliseconds;

                        // Calcola la probabilità di errore crescente logaritmicamente
                        failureProbability = CalculateFailureProbability(elapsedTime, failureGrowthFactor);

                        // Log del messaggio in base alla probabilità
                        LogMessageBasedOnProbability(logger, failureProbability);

                        // Se la probabilità è abbastanza alta, generiamo un errore fatale
                        if (_random.NextDouble() < failureProbability)
                        {
                            logger.Log(LogLevel.Fatal, $"CRASH! Errore fatale al {elapsedTime} ms. Il sistema si sta arrestando!");
                            throw new Exception("Errore fatale: il sistema è stato compromesso.");
                        }

                        // Simula il passare del tempo con una breve pausa
                        Thread.Sleep(100);
                    }

                    logger.Log(LogLevel.Info, "La simulazione ha terminato senza errori fatali.");
                }
                catch (Exception ex)
                {
                    logger.Log(LogLevel.Fatal, $"Il thread ha terminato a causa di un errore fatale: {ex.Message}");
                }
            });

            // Avvia il thread
            simulationThread.Start();
        }

        /// <summary>
        /// Calcola la probabilità di errore fatale in base al tempo trascorso, seguendo una curva logaritmica.
        /// </summary>
        /// <param name="elapsedTime">Il tempo trascorso in millisecondi.</param>
        /// <param name="growthFactor">Il fattore che controlla la crescita della probabilità.</param>
        /// <returns>La probabilità di errore calcolata.</returns>
        private static double CalculateFailureProbability(int elapsedTime, double growthFactor)
        {
            return Math.Log(elapsedTime + 1) / growthFactor;
        }

        /// <summary>
        /// Registra i messaggi di log in base alla probabilità di errore.
        /// </summary>
        /// <param name="logger">Il logger utilizzato per registrare i messaggi.</param>
        /// <param name="failureProbability">La probabilità di errore calcolata.</param>
        private static void LogMessageBasedOnProbability(ILogger logger, double failureProbability)
        {
            if (failureProbability < 0.05)
                logger.Log(LogLevel.Trace, "Sistema stabile. Nessun problema.");
            else if (failureProbability < 0.1)
                logger.Log(LogLevel.Info, "Leggeri problemi riscontrati.");
            else if (failureProbability < 0.2)
                logger.Log(LogLevel.Warning, "Anomalie rilevate.");
            else if (failureProbability < 0.5)
                logger.Log(LogLevel.Error, "Problemi gravi in corso.");
        }
    }
}