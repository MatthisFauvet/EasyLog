using EasyLog.entity;
using EasyLog.writers;

namespace EasyLog;

public class Logger
{
    
    private readonly List<ILogWriter> _writers = new List<ILogWriter>();

    public Logger()
    {
        
    }

    public void InitWriters()
    {
        _writers.Add(new ConsoleLogWriter());
    }
    
    public void AddWriter(ILogWriter writer)
    {
        _writers.Add(writer);
    }

    public void Log(string message, LogType type = LogType.Info)
    {
        foreach (var writer in _writers)
        {
            writer.write(type, message, new DateTime());
        }
    }
    
}