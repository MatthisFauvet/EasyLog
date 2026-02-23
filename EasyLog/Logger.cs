using EasyLog.entity;
using EasyLog.writers;
using System.Collections.Concurrent;

namespace EasyLog;

public class Logger
{
    // ConcurrentBag allows safe concurrent reads and writes
    // without needing a manual lock on the collection itself
    // We use a snapshot pattern for iteration (explained below)
    private readonly List<ILogWriter> _writers = new List<ILogWriter>();
    private readonly object _writersLock = new object();

    public Logger()
    {
        
    }

    public void InitWriters(string fileDirectory, string context)
    {
        lock (_writersLock)
        {
            _writers.Add(new ConsoleLogWriter());
            _writers.Add(new FileLogWriter(fileDirectory, context));
            _writers.Add(new JsonFileWriter(fileDirectory, context));
            _writers.Add(new XmlFileWriter(fileDirectory, context));
        }
    }
    
    public void AddWriter(ILogWriter writer)
    {
        // Lock so we don't modify the list while Log() is iterating it
        lock (_writersLock)
        {
            _writers.Add(writer);
        }
    }

    public void Log(Dictionary<string, string> messageByTopic, LogType type = LogType.Info)
    {
        // Take a snapshot of the list under lock
        // This means we release the lock quickly and don't block AddWriter()
        // for the entire duration of writing to all destinations
        ILogWriter[] snapshot;
        lock (_writersLock)
        {
            snapshot = _writers.ToArray();
        }

        string timeStamp = DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");

        // Write to all writers in parallel — safe because each writer
        // is already internally thread-safe from our previous changes
        Parallel.ForEach(snapshot, writer =>
        {
            writer.write(type, timeStamp, messageByTopic);
        });
    }
}