using EasyLog.entity;
using EasyLog.writers;

namespace EasyLog;

public class Logger : IDisposable
{
    private readonly List<ILogWriter> _writers = new List<ILogWriter>();

    public Logger() { }

    public void InitWriters(string fileDirectory, string context)
    {
        _writers.Add(new ConsoleLogWriter());
        _writers.Add(new FileLogWriter(fileDirectory, context));
        _writers.Add(new JsonFileWriter(fileDirectory, context));
        _writers.Add(new XmlFileWriter(fileDirectory, context));
    }

    public void AddWriter(ILogWriter writer)
    {
        _writers.Add(writer);
    }

    public void Log(Dictionary<string, string> messageByTopic, LogType type = LogType.Info)
    {
        foreach (var writer in _writers)
        {
            writer.write(type, DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss"), messageByTopic);
        }
    }

    /// <summary>
    /// Disposes all writers that hold unmanaged resources (e.g. open file handles).
    /// </summary>
    public void Dispose()
    {
        foreach (var writer in _writers)
        {
            if (writer is IDisposable disposable)
                disposable.Dispose();
        }
        _writers.Clear();
    }
}