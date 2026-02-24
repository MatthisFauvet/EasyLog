using EasyLog.entity;

namespace EasyLog.writers;

public class ConsoleLogWriter : ILogWriter
{
    // The lock object — one instance shared across all calls to this writer
    // It must be static so all threads share the SAME lock
    private static readonly object _consoleLock = new object();
    public ConsoleLogWriter()
    {
    }

    public void write(LogType logType, string timeStamp, Dictionary<string, string> message)
    {
        // Everything inside this lock runs atomically — one thread at a time
        // Other threads that reach this point will WAIT until the current one exits
        lock (_consoleLock)
        {
            // Couleur selon le niveau de log
            switch (logType)
            {
                case LogType.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogType.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogType.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
            }

            // Ligne principale
            Console.WriteLine($"{logType} - {timeStamp}");

            // Détails du dictionnaire
            foreach (var kvp in message)
            {
                Console.WriteLine($"  - {kvp.Key} : {kvp.Value}");
            }

            Console.WriteLine(); // séparation entre logs

            // Reset couleur console
            Console.ResetColor();
        }
    }
}