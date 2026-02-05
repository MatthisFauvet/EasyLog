using EasyLog.entity;

namespace EasyLog.writers;

public class FileLogWriter : ILogWriter
{

    private string _fileName;

    private string _filePath;

    private string _context;

    private StreamWriter _streamWriter; 

    
    /// <summary>
    /// This class allow user to create text log file 
    /// </summary>
    /// <param name="fileDirectory">The destination path for the file</param>
    /// <param name="context">The context can be a description, a name. It's use to help you find and name log file</param>
    public FileLogWriter(string fileDirectory, string context)
    {
        _context = context;
        _filePath = ComputeFilePath(fileDirectory, context);
        InitFileStream();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="logType">type of log</param>
    /// <param name="message">core message : Exception, Message, Information</param>
    /// <param name="timeStamp">When did the error occured</param>
    public void write(LogType logType, string message, string timeStamp)
    {
        try
        {
            _streamWriter.WriteLine($"[{logType}] - {timeStamp} - {message}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    private void InitFileStream()
    {
        try
        {
            FileStream fs = File.Create(_filePath);
            _streamWriter = new StreamWriter(fs);
            _streamWriter.AutoFlush = true;
        } 
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    private string ComputeFilePath(string fileDirectory, string context)
    {
        return Path.Combine(fileDirectory, ComputeFileName(context));
    }

    private string ComputeFileName(string context)
    {
        return context + "-" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + ".txt";
    }
}