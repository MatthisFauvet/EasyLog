using EasyLog.entity;

namespace EasyLog.writers;

public class ConsoleLogWriter : ILogWriter
{
    public ConsoleLogWriter()
    {
    }

    public void write(LogType logType, string timeStamp, Dictionary<string, string> message)
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