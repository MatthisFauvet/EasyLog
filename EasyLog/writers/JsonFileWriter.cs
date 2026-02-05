using System.Text.Json;
using EasyLog.entity;

namespace EasyLog.writers;

public class JsonFileWriter : ILogWriter, IDisposable
{
    private readonly string _context;
    private readonly string _filePath;
    private readonly StreamWriter _streamWriter;

    public JsonFileWriter(string fileDirectory, string context)
    {
        _context = context;
        _filePath = ComputeFilePath(fileDirectory, context);

        _streamWriter = new StreamWriter(_filePath, append: true)
        {
            AutoFlush = true
        };
    }

    public void write(LogType logType, string message, string timeStamp)
    {
        try
        {
            var logEntry = new
            {
                timestamp = timeStamp,
                level = logType.ToString(),
                context = _context,
                message = message
            };

            string json = JsonSerializer.Serialize(logEntry);
            _streamWriter.WriteLine(json);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public void Dispose()
    {
        _streamWriter?.Dispose();
    }

    private string ComputeFilePath(string fileDirectory, string context)
    {
        return Path.Combine(fileDirectory, ComputeFileName(context));
    }

    private string ComputeFileName(string context)
    {
        return $"{context}-{DateTime.Now:dd-MM-yyyy}.jsonl";
    }
}