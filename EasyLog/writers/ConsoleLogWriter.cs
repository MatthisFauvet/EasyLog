using EasyLog.entity;

namespace EasyLog.writers;

public class ConsoleLogWriter : ILogWriter
{
    
    public ConsoleLogWriter()
    {
        
    }

    public void write(LogType logType, string message, DateTime timeStamp)
    {
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
        }
        Console.WriteLine($"{logType} - {timeStamp.ToString()} - {message}");
    }
}